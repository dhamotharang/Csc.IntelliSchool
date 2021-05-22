using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    public SingleEmployeeEarningSummary[] GetSingleEmployeeEarningsSummary(int employeeId, int year, PeriodFilter filter) {
      DateTime startDate = DateTime.MinValue;
      DateTime endDate = DateTime.MaxValue;
      DateTimeExtensions.GetMonthOverlapFilterRange(new DateTime(year, 1, 1), filter, out startDate, out endDate);

      using (var ent = CreateModel()) {
        // Clear any portential duplicates 
        ent.EmployeeDeleteDuplicateEarnings(startDate, endDate, new IntList(employeeId.PackArray()));

        var earnings = ent.EmployeeEarnings
          .Query(new EmployeeRangeDataCriteria() { EmployeeIDs = employeeId.PackArray() }.SetRange(startDate, endDate).As<EmployeeRangeDataCriteria>()).ToArray();

        return earnings.Select(s => SingleEmployeeEarningSummary.Create(s)).ToArray();

      }
    }

    public EmployeeEarningSummary[] GetEmployeeEarningsSummary(DateTime startMonth, DateTime endMonth, int[] listIds) {
      DateTime startDate = startMonth.ToMonth();
      DateTime endDate = endMonth.ToMonthEnd();


      using (var ent = CreateModel()) {
        var allEarnings = InternalGetEmployeeEarnings(ent, startDate, endDate, listIds, EmployeeIncludes.EmployeeList);
        var employees = allEarnings.Select(s => s.Employee).Where(s=>s!=null).Distinct().ToArray();

        List<EmployeeEarningSummary> results = new List<EmployeeEarningSummary>(employees.Count());
        foreach (var emp in employees) {
          var earnings = allEarnings.Where(s => s.EmployeeID == emp.EmployeeID && emp.IsMonthEmployee(s.Month)).ToArray();
          if (earnings.Count() == 0)
            continue;

          results.Add(EmployeeEarningSummary.Create(emp, earnings));
        }

        return results.ToArray();
      }

    }
  }
}