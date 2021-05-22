using System;
using System.IO;
using System.Linq;
using Csc.Components.Common;
using System.Collections.Generic;
using Csc.IntelliSchool.Data;
using Csc.Components.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    public void RecalculateEmployeeEarnings(DateTime month, int[] employeeIds, EmployeeRecalculateFlags optionFlags) {
      //////////////////////
      // 1. init
      optionFlags |= EmployeeRecalculateFlags.Earning;

      month = month.ToMonth();
      DateTime startDate = month;
      DateTime endDate = startDate.ToMonthEnd();

      using (var ent = CreateModel(false)) {
        //////////////////////
        // 2. remove any duplicates
        ent.EmployeeDeleteDuplicateEarnings(startDate, endDate, new IntList(employeeIds));

        HashSet<EmployeeEarning> earningList = ent.EmployeeEarnings
          .Include(DataExtensions.GetIncludes(EmployeeIncludes.Employment | EmployeeIncludes.ShiftOverridesTerminal | EmployeeIncludes.Salary, "Employee"))
          .Query(new EmployeeRangeDataCriteria() { EmployeeIDs = employeeIds }.SetMonth(month).As<EmployeeRangeDataCriteria>()).ToHashSet();


        InternalCalculateEmployeeEarnings(month, employeeIds, optionFlags, earningList, ent);
      }
    }

    public EmployeeEarning[] GetEmployeeEarnings(DateTime month, int[] listIds, int[] employeeIds, EmploeeEarningCalculationMode mode) {
      //////////////////////
      // 1. init
      month = month.ToMonth();
      DateTime startDate = month;
      DateTime endDate = startDate.ToMonthEnd();

      using (var ent = CreateModel(false)) {
        //////////////////////
        // 2. select valid employee ids
        employeeIds = ent.Employees.Query(EmployeeIncludes.None,
          new EmployeeDataCriteria() { StartDate = month.ToMonth(), EndDate = month.ToMonthEnd(), ListIDs = listIds, EmployeeIDs = employeeIds })
          .Select(s => s.EmployeeID).ToArray();

        //////////////////////
        // 3. remove any duplicates
        ent.EmployeeDeleteDuplicateEarnings(startDate, endDate, new IntList(employeeIds));

        //////////////////////
        // 4. load earnings
        // TODO: review includes
        HashSet<EmployeeEarning> earningList = ent.EmployeeEarnings
          .Include(DataExtensions.GetIncludes(EmployeeIncludes.Personal | EmployeeIncludes.Employment | EmployeeIncludes.ShiftOverridesTerminal | EmployeeIncludes.Salary, "Employee"))
          .Query(new EmployeeRangeDataCriteria() { EmployeeIDs = employeeIds }.SetMonth(month).As<EmployeeRangeDataCriteria>()).ToHashSet();

        //////////////////////
        // get recalc employees
        if (mode == EmploeeEarningCalculationMode.None) // recalc not found if None is selected
          employeeIds = employeeIds.Except(earningList.Select(s => s.EmployeeID).ToArray()).ToArray();

        //////////////////////
        // recalc
        var flags = EmployeeRecalculateFlags.EditedEarning;
        if (mode == EmploeeEarningCalculationMode.Basic)
          flags |= EmployeeRecalculateFlags.EarningSalariesOnly;

        InternalCalculateEmployeeEarnings(month, employeeIds, flags, earningList, ent);


        return earningList.ToArray();
      }
    }


    private static EmployeeEarning[] InternalGetEmployeeEarnings(DataEntities ent, DateTime startDate, DateTime endDate, int[] listIds, EmployeeIncludes includes) {
      var employeeIds = ent.Employees.Query(new EmployeeDataCriteria() { StartDate = startDate, EndDate = endDate, ListIDs = listIds })
        .Select(s=>s.EmployeeID).ToArray();

      // Clear any portential duplicates 
      ent.EmployeeDeleteDuplicateEarnings(startDate, endDate, new IntList(employeeIds));

      return ent.EmployeeEarnings
        .Include(DataExtensions.GetIncludes(includes, "Employee"))
        .Query(new EmployeeRangeDataCriteria() { EmployeeIDs = employeeIds }.SetRange(startDate, endDate).As<EmployeeRangeDataCriteria>())
        .ToArray();
    }



    private void InternalCalculateEmployeeEarnings(DateTime month, int[] employeeIds, EmployeeRecalculateFlags flags, HashSet<EmployeeEarning> earningList, DataEntities ent) {
      if (employeeIds.Count() == 0)
        return;

      //////////////////////
      // load employees to recalculate
      var employees = ent.Employees
        .Query(EmployeeIncludes.Personal | EmployeeIncludes.Employment | EmployeeIncludes.ShiftOverridesTerminal | EmployeeIncludes.Salary,
        new EmployeeDataCriteria() { EmployeeIDs = employeeIds }).ToArray();

      //////////////////////
      // fill options
      EmployeeCalculateOptions options = new EmployeeCalculateOptions(flags);
      options.Month = month;

      //////////////////////
      // fill state
      EmployeeCalculateEarningState operationState = new EmployeeCalculateEarningState(ent);
      InternalLoadCalculateEarningState(employees, options, operationState);

      operationState.Attendance = InternalGetEmployeeAttendance(options, employeeIds).ToArray();
      operationState.Earnings = earningList;

      //////////////////////
      // calculate
      foreach (var emp in employees) {
        InternalCalculateSingleEmployeeEarning(emp, options, operationState);
      }

      //////////////////////
      // log
      ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Calculate, typeof(EmployeeEarning), null,
        new SystemLogDataEntry() { EmployeeIDs = employees.Select(s => s.EmployeeID).ToArray(), EmployeeRecalculateFlags = options.OptionFlags, Month = options.Month });

      ent.SaveChanges();
    }

    public EmployeeEarning UpdateEmployeeEarning(EmployeeEarning earning) {
      earning.IsEdited = true;

      using (var ent = CreateModel()) {
        // ensuring there's no duplicates
        ent.EmployeeDeleteDuplicateAttendance(earning.Month, earning.Month, (IntList)earning.EmployeeID.PackArray());

        var dbEarning = ent.EmployeeEarnings.SingleOrDefault(s => s.EmployeeID == earning.EmployeeID && s.Month == earning.Month);

        if (dbEarning == null) {
          earning.EarningID = 0;
          ent.EmployeeEarnings.Add(earning);
        }


        ent.Entry(dbEarning).CurrentValues.SetValues(earning);

        ent.SaveChanges();

        ent.Logger.LogDatabase(CurrentUser, dbEarning != null ? SystemLogDataAction.Update : SystemLogDataAction.Insert, typeof(EmployeeEarning), null,
          new SystemLogDataEntry() { EmployeeID = earning.EmployeeID, Month = earning.Month });
        ent.Logger.Flush();

        return dbEarning == null ? earning : dbEarning;
      }
    }
  }
}
