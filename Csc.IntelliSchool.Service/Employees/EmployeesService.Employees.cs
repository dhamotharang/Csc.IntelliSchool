using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Query
    /// <summary>
    /// Returns month employees (active/inactive) for the selected month
    /// </summary>
    public Employee[] GetMonthEmployees(EmployeeIncludes filter, DateTime month, int[] listIds, int[] employeeIds) {
      month = month.ToMonth();
      using (var ent = CreateModel()) {
        return ent.Employees.Query(filter, 
          new EmployeeDataCriteria() {
            StartDate = month.ToMonth(),
            EndDate = month.ToMonthEnd(),
            ListIDs = listIds 
          }).ToArray();
      }
    }


    /// <summary>
    /// Returns terminated employees
    /// </summary>
    public Employee[] GetTerminatedEmployees(DateTime? month, EmployeeIncludes filter, int[] listIds) {
      month = month.ToMonth();
      using (var ent = CreateModel()) {
        var qry = ent.Employees.Query(filter, new EmployeeDataCriteria() { ListIDs = listIds });

        if (month == null)
          qry = qry.Where(s => s.TerminationDate != null);
        else {
          DateTime startDate = month.ToMonth().Value;
          DateTime endDate = month.ToMonthEnd().Value;

          qry = qry.Where(s => s.TerminationDate >= startDate && s.TerminationDate <= endDate);
        }

        return qry.ToArray();
      }
    }

    #endregion



    #region Update
    /// <summary>
    /// Adds/Updates employee and returns added/updated item with default data
    /// </summary>
    public Employee AddOrUpdateEmployee(Employee userItem) {
      foreach (var itm in userItem.OfficialDocuments) {
        itm.Employee = null;
        itm.Type = null;
      }

      using (var ent = CreateModel()) {
        var dbItem = ent.Employees.Query(EmployeeIncludes.EmployeeList | EmployeeIncludes.OfficialDocuments).SingleOrDefault(s => s.EmployeeID == userItem.EmployeeID);

        if (dbItem != null) { // update
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);

          PeopleService.Instance.InternalAddOrUpdatePerson(ent, userItem.Person);

          ent.UpdateChildEntities(dbItem.OfficialDocuments.ToArray(), userItem.OfficialDocuments.ToArray(),
            (a, b) => a.EmployeeID == b.EmployeeID && a.TypeID == b.TypeID,
            DbOperation.Add | DbOperation.Update);

          if (userItem.Salary != null && (dbItem.Salary == null ||
            (dbItem.Salary != null &&
            dbItem.Salary.Salary != userItem.Salary.Salary ||
            dbItem.Salary.Social != userItem.Salary.Social ||
            dbItem.Salary.Medical != userItem.Salary.Medical ||
            dbItem.Salary.Taxes != userItem.Salary.Taxes))) {

            dbItem.SalaryUpdates.Add(new EmployeeSalaryUpdate() { // same date
              Salary = userItem.Salary.Salary,
              Social = userItem.Salary.Social,
              Medical = userItem.Salary.Medical,
              Taxes = userItem.Salary.Taxes,
            });
          }

          ent.UpdateRelatedEntity(dbItem.Salary, userItem.Salary);// Salary

        } else {
          ent.Employees.Add(userItem);
        }

        ent.SaveChanges();

        ent.Logger.LogDatabase(CurrentUser, dbItem != null ? SystemLogDataAction.Update : SystemLogDataAction.Insert, typeof(Employee), userItem.EmployeeID.PackArray());
        ent.Logger.Flush();

        return ent.Employees.Query(EmployeeIncludes.EmployeeList).SingleOrDefault(s => s.EmployeeID == userItem.EmployeeID);
      }
    }


    public Employee TerminateEmployee(Employee item) {
      using (var ent = CreateModel()) {
        var dbItem = ent.Employees.SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
        dbItem.TerminationDate = item.TerminationDate;
        dbItem.TerminationReason = item.TerminationReason;

        ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Terminate, typeof(Employee), item.EmployeeID.PackArray(),
          new SystemLogDataEntry(item) { Date = item.TerminationDate });

        ent.SaveChanges();

        return ent.Employees.Query(EmployeeIncludes.EmployeeList).SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
      }
    }
    public Employee ReenrollEmployee(Employee item) {
      using (var ent = CreateModel()) {
        var dbItem = ent.Employees.SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
        dbItem.HireDate = item.HireDate;
        dbItem.TerminationDate = null;
        dbItem.TerminationReason = null;

        ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Reenroll, typeof(Employee), item.EmployeeID.PackArray(),
          new SystemLogDataEntry(item) { Date = item.HireDate });

        ent.SaveChanges();

        return ent.Employees.Query(EmployeeIncludes.EmployeeList).SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
      }
    }

    public bool CheckEmployeeTerminalUsed(int employeeId, int terminalId, int userId) {
      using (var ent = CreateModel()) {
        var itm = ent.Employees.Where(s => s.EmployeeID != employeeId && s.TerminalID == terminalId && s.TerminalUserID == userId).FirstOrDefault();
        return itm != null && itm.IsTerminated == false;
      }
    }

    public EmployeeSalaryUpdate[] GetEmployeeSalaryUpdates(int employeeId, int year, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        var qry = ent.EmployeeSalaryUpdates.Where(s => s.EmployeeID == employeeId);

        switch (filter) {
          case PeriodFilter.Current:
            qry = qry.Where(s => s.Date.Year == year);
            break;
          case PeriodFilter.Upcoming:
            qry = qry.Where(s => s.Date.Year > year);
            break;
          case PeriodFilter.Past:
            qry = qry.Where(s => s.Date.Year < year);
            break;
          default:
            break;
        }

        var items = qry.OrderBy(s => s.Date).ToArray();

        for (int i = 1; i < items.Count(); i++) {
          var prevItem = items.ElementAt(i - 1);
          var item = items.ElementAt(i);

          item.PreviousSalary = prevItem.Salary;
        }

        return items;
      }
    }
    #endregion

  }

}

