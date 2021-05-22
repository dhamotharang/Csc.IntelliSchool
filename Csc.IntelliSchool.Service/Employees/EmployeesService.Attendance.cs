using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    public EmployeeAttendance UpdateEmployeeAttendance(EmployeeAttendance userItem) {
      using (var ent = CreateModel()) {
        var dbItem = ent.EmployeeAttendance.Query(EmployeeAttendanceIncludes.TimeOffs).SingleOrDefault(s => s.AttendanceID == userItem.AttendanceID);
        ent.Entry(dbItem).CurrentValues.SetValues(userItem);
        ent.UpdateChildEntities(dbItem.TimeOffs.ToArray(), userItem.TimeOffs.ToArray(), (a, b) => a.TimeOffID == b.TimeOffID);

        dbItem.IsEdited = true;

        ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Update, typeof(EmployeeAttendance), userItem.AttendanceID.PackArray());

        ent.SaveChanges();

        return ent.EmployeeAttendance.Query(EmployeeAttendanceIncludes.TimeOffs).SingleOrDefault(s => s.AttendanceID == userItem.AttendanceID);
      }
    }

    public EmployeeAttendance[] GetSingleEmployeeAttendance(int employeeId, DateTime month, EmployeeRecalculateFlags flags) {
      EmployeeCalculateOptions options = new EmployeeCalculateOptions(flags);
      options.Month = month;
      return InternalGetEmployeeAttendance(options, new int[] { employeeId }).ToArray();
    }

    public void RecalculateEmployeeMonthlyEarning(DateTime month, int[] listIds, EmployeeRecalculateFlags flags) {
      int[] employeeIds = new int[] { };
      using (var ent = CreateModel()) {
        employeeIds = ent.Employees.Query(EmployeeIncludes.None,
          new EmployeeDataCriteria() { StartDate = month.ToMonth(), EndDate = month.ToMonthEnd(), ListIDs = listIds })
          .Select(s => s.EmployeeID).ToArray();
      }

      if (employeeIds.Count() == 0)
        return;

      if (flags.HasFlag(EmployeeRecalculateFlags.Attendance))
        RecalculateEmployeeAttendance(month, employeeIds, flags);

      if (flags.HasFlag(EmployeeRecalculateFlags.Earning))
        RecalculateEmployeeEarnings(month, employeeIds, flags);
    }

    public void RecalculateEmployeeAttendance(DateTime month, int[] employeeIds, EmployeeRecalculateFlags flags) {
      flags = flags | EmployeeRecalculateFlags.Attendance;

      EmployeeCalculateOptions options = new EmployeeCalculateOptions(flags);
      options.Month = month;

      InternalGetEmployeeAttendance(options, employeeIds);
    }

    public EmployeeAttendanceObject[] GetEmployeeAttendance
      (int? branchId, int? departmentId, int? positionId, int[] listIds,
      DateTime month, EmployeeRecalculateFlags flags) {

      using (var ent = CreateModel()) {
        var employees = ent.Employees.Query(EmployeeIncludes.Employment | EmployeeIncludes.Person,
          new EmployeeDataCriteria() {
            StartDate = month.ToMonth(), EndDate = month.ToMonthEnd(), ListIDs = listIds,
            BranchIDs = branchId != null ? new int[] { branchId.Value } : new int[] { },
            DepartmentIDs = departmentId != null ? new int[] { departmentId.Value } : new int[] { },
            PositionIDs = positionId != null ? new int[] { positionId.Value } : new int[] { },
          }).ToArray();


        EmployeeCalculateOptions options = new EmployeeCalculateOptions(flags);
        options.Month = month;
        EmployeeCalculateAttendanceState state = new EmployeeCalculateAttendanceState(ent);

        var allAttendance = InternalGetEmployeeAttendance(options, state, employees);

        return employees.Select(emp =>
          new EmployeeAttendanceObject() {
            Employee = emp,
            Attendance = allAttendance.Where(s => s.EmployeeID == emp.EmployeeID).ToArray()
          }).ToArray();
      }
    }


    private IEnumerable<EmployeeAttendance> InternalGetEmployeeAttendance(EmployeeCalculateOptions options, int[] employeeIds) {
      using (var ent = CreateModel()) {
        return InternalGetEmployeeAttendance(options,
          new EmployeeCalculateAttendanceState(ent),
          employeeIds);
      }
    }
    private IEnumerable<EmployeeAttendance> InternalGetEmployeeAttendance(EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state, int[] employeeIds) {
      var employees = state.Model.Employees.Where(s => employeeIds.Contains(s.EmployeeID)).ToArray();
      return InternalGetEmployeeAttendance(options, state, employees);
    }
    private IEnumerable<EmployeeAttendance> InternalGetEmployeeAttendance(EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state, Employee[] allEmployees) {
      int[] allEmployeeIds = allEmployees.Select(s => s.EmployeeID).ToArray();

      HashSet<EmployeeAttendance> attendanceList = InternalGetAttendanceItems(options, state, allEmployeeIds);

      int[] recalcIds = options.CalculateAttendance ? allEmployeeIds :
        attendanceList.GroupBy(s => s.EmployeeID).Where(s => s.Count() != options.EndDate.Day).Select(s => s.Key).ToArray() // if day count is not the same
        .Concat(allEmployeeIds.Except(attendanceList.Select(s => s.EmployeeID))).Distinct().ToArray();

      if (recalcIds.Count() > 0) {
        var recalcEmployees = allEmployees.Where(s => recalcIds.Contains(s.EmployeeID)).ToArray();

        options.OptionFlags |= EmployeeRecalculateFlags.Attendance;
        InternalLoadCalculateAttendanceState(recalcEmployees, options, state);
        state.Attendance = attendanceList;

        var watch = System.Diagnostics.Stopwatch.StartNew();
        foreach (var emp in recalcEmployees) {
          InternalCalculateSingleEmployeeAttendance(emp, options, state);
        }
        watch.Stop();
        Trace.WriteLine("Time= " + new TimeSpan(watch.ElapsedTicks).ToString()); ;

        state.Model.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Calculate, typeof(EmployeeAttendance), null,
          new SystemLogDataEntry() {
            EmployeeIDs = recalcEmployees.Select(s => s.EmployeeID).ToArray(), EmployeeRecalculateFlags = options.OptionFlags, Month = options.Month
          });

        state.Model.SaveChanges();
      }

      return attendanceList.OrderBy(s => s.EmployeeID).ThenBy(s => s.Date).ToArray();
    }

    private static HashSet<EmployeeAttendance> InternalGetAttendanceItems(EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state, int[] newEmployeeIds) {
      state.Model.EmployeeDeleteDuplicateAttendance(options.StartDate, options.EndDate, new IntList(newEmployeeIds));

      return state.Model.EmployeeAttendance.Query(EmployeeAttendanceIncludes.TimeOffs)
        .Where(s => newEmployeeIds.Contains(s.EmployeeID) && s.Date >= options.StartDate && s.Date <= options.EndDate).ToHashSet();
    }
  }
}
