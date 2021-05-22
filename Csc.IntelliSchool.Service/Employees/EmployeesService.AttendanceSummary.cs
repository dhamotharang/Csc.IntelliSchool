using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    public EmployeeAttendanceSummary[] GetEarningsAttendanceSummary(DateTime startMonth, DateTime endMonth, int[] listIds) {
      DateTime startDate = startMonth.ToMonth();
      DateTime endDate = endMonth.ToMonthEnd();

      using (var ent = CreateModel()) {
        var allEarnings = InternalGetEmployeeEarnings(ent, startDate, endDate, listIds, EmployeeIncludes.EmployeeList);
        var employees = allEarnings.Select(s => s.Employee).Where(s => s != null).Distinct().ToArray();

        List<EmployeeAttendanceSummary> results = new List<EmployeeAttendanceSummary>(employees.Count());
        foreach (var emp in employees) {
          var earnings = allEarnings.Where(s => s.EmployeeID == emp.EmployeeID && emp.IsMonthEmployee(s.Month)).ToArray();
          if (earnings.Count() == 0)
            continue;

          results.Add(EmployeeAttendanceSummary.Create(emp, earnings));
        }

        return results.ToArray();
      }

    }

    public EmployeeAttendanceSummary[] GetAttendanceSummary(DateTime startMonth, DateTime endMonth, int[] listIds) {
      DateTime startDate = startMonth.ToMonth();
      DateTime endDate = endMonth.ToMonthEnd();

      using (var ent = CreateModel()) {
        var employees = ent.Employees.Query(EmployeeIncludes.EmployeeList, new EmployeeDataCriteria() { StartDate = startDate, EndDate = endDate, ListIDs = listIds }).ToArray();
        var employeeIds = employees.Select(s => s.EmployeeID).ToArray();
        var allAttendance = ent.EmployeeAttendance
          .Query(EmployeeAttendanceIncludes.TimeOffs, new EmployeeRangeDataCriteria() { EmployeeIDs = employeeIds, StartDate = startDate, EndDate = endDate }).ToArray();

        //TODO: Flags auto load

        List<EmployeeAttendanceSummary> results = new List<EmployeeAttendanceSummary>(employees.Count());
        foreach (var emp in employees) {
          var attendance = allAttendance.Where(s => s.EmployeeID == emp.EmployeeID && emp.IsMonthEmployee(s.Date)).ToArray();
          if (attendance.Count() == 0)
            continue;

          results.Add(EmployeeAttendanceSummary.Create(emp, attendance));
        }

        return results.ToArray();
      }

    }

  }
}