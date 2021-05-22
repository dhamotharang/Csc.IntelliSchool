using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Query
    /// <summary>
    /// Returns current employees (active/inactive) for the selected month
    /// </summary>
    public Employee[] GetCurrentEmployees(EmployeeDataFilter filter, DateTime month, int[] listIds, int[] employeeIds) {
      month = month.ToMonth();
      using (var ent = ServiceManager.CreateModel()) {
        return ent.Employees.Query(filter, month, listIds, employeeIds).ToArray();
      }
    }


    /// <summary>
    /// Returns all terminated employees
    /// </summary>
    public Employee[] GetTerminatedEmployees(DateTime? month, EmployeeDataFilter filter, int[] listIds) {
      month = month.ToMonth();
      using (var ent = ServiceManager.CreateModel()) {
        var qry = ent.Employees.Query(filter, null, listIds);

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

      using (var ent = ServiceManager.CreateModel()) {
        var dbItem = ent.Employees.Query(EmployeeDataFilter.Default | EmployeeDataFilter.OfficialDocuments).SingleOrDefault(s => s.EmployeeID == userItem.EmployeeID);

        if (dbItem != null) { // update
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);

          ent.UpdateRelatedEntity(dbItem.Contact, userItem.Contact);// Contact
          if (userItem.Contact != null) {
            ent.UpdateChildEntities( dbItem.Contact.Numbers.ToArray(), userItem.Contact.Numbers.ToArray(), (a, b) => a.NumberID == b.NumberID);
            ent.UpdateChildEntities( dbItem.Contact.Addresses.ToArray(), userItem.Contact.Addresses.ToArray(), (a, b) => a.AddressID == b.AddressID);
          }

          ent.UpdateChildEntities( dbItem.OfficialDocuments.ToArray(), userItem.OfficialDocuments.ToArray(),
            (a, b) => a.EmployeeID == b.EmployeeID && a.TypeID == b.TypeID,
            DbOperation.Add | DbOperation.Update);

          if (dbItem.Salary == null ||
            (dbItem.Salary != null &&
            dbItem.Salary.Salary != userItem.Salary.Salary ||
            dbItem.Salary.Housing != userItem.Salary.Housing ||
            dbItem.Salary.Travel != userItem.Salary.Travel ||
            dbItem.Salary.Social != userItem.Salary.Social ||
            dbItem.Salary.Medical != userItem.Salary.Medical ||
            dbItem.Salary.Taxes != userItem.Salary.Taxes ||
            dbItem.Salary.Allowance != userItem.Salary.Allowance ||
            dbItem.Salary.Expenses != userItem.Salary.Expenses)) {

            dbItem.SalaryUpdates.Add(new EmployeeSalaryUpdate() { // same date
              Salary = userItem.Salary.Salary,
              Housing = userItem.Salary.Housing,
              Travel = userItem.Salary.Travel,
              Social = userItem.Salary.Social,
              Medical = userItem.Salary.Medical,
              Taxes = userItem.Salary.Taxes,
              Allowance = userItem.Salary.Allowance,
              Expenses = userItem.Salary.Expenses,
            });
          }

          ent.UpdateRelatedEntity(dbItem.Salary, userItem.Salary);// Salary

        } else {
          ent.Employees.Add(userItem);
        }

        ent.SaveChanges();

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbItem != null ? SystemLogDataAction.Update : SystemLogDataAction.Insert, typeof(Employee), userItem.EmployeeID.PackArray());
        ent.Logger.Flush();

        return ent.Employees.Query(EmployeeDataFilter.Default).SingleOrDefault(s => s.EmployeeID == userItem.EmployeeID);
      }
    }


    public Employee TerminateEmployee(Employee item) {
      using (var ent = ServiceManager.CreateModel()) {
        var dbItem = ent.Employees.SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
        dbItem.TerminationDate = item.TerminationDate;
        dbItem.TerminationReason = item.TerminationReason;

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Terminate, typeof(Employee), item.EmployeeID.PackArray(),
          new SystemLogDataEntry(item) { Date = item.TerminationDate });

        ent.SaveChanges();

        return ent.Employees.Query(EmployeeDataFilter.Default).SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
      }
    }
    public Employee ReenrollEmployee(Employee item) {
      using (var ent = ServiceManager.CreateModel()) {
        var dbItem = ent.Employees.SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
        dbItem.HireDate = item.HireDate;
        dbItem.TerminationDate = null;
        dbItem.TerminationReason = null;

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Reenroll, typeof(Employee), item.EmployeeID.PackArray(),
          new SystemLogDataEntry(item) { Date = item.HireDate });

        ent.SaveChanges();

        return ent.Employees.Query(EmployeeDataFilter.Default).SingleOrDefault(s => s.EmployeeID == item.EmployeeID);
      }
    }

    public bool CheckEmployeeTerminalUsed(int employeeId, int terminalId, int userId) {
      using (var ent = ServiceManager.CreateModel()) {
        var itm = ent.Employees.Where(s => s.EmployeeID != employeeId && s.TerminalID == terminalId && s.TerminalUserID == userId).FirstOrDefault();
        return itm != null && itm.IsTerminated == false;
      }
    }

    public EmployeeSalaryUpdate[] GetEmployeeSalaryUpdates(int employeeId, int year, PeriodFilter filter) {
      using (var ent = ServiceManager.CreateModel()) {
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

