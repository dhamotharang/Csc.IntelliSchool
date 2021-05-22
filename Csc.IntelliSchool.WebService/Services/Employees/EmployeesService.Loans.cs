using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {

    #region Loans
    public EmployeeLoan[] GetEmployeeLoansByPeriod(int[] employeeIds, DateTime month, PeriodFilter filter, bool includeEmployee) {
      month = new DateTime(month.Year, month.Month, 1);

      using (var ent = ServiceManager.CreateModel()) {
        var qry = ent.EmployeeLoans.Query(includeEmployee);

        if (employeeIds != null) {
          qry = qry.Where(s => employeeIds.Contains(s.EmployeeID));
        }

        if (filter == PeriodFilter.Current)
          return qry.Where(s => s.Installments.Where(x => x.Month == month).Count() > 0).ToArray();
        else if (filter == PeriodFilter.Upcoming)
          return qry.Where(s => s.Installments.Where(x => x.Month > month).Count() > 0).ToArray();
        else if (filter == PeriodFilter.Past)
          return qry.Where(s => s.Installments.Where(x => x.Month < month).Count() > 0).ToArray();
        else
          return null;
      }
    }

    public EmployeeLoan[] GetEmployeeLoans(DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeLoans.Query(startDate, endDate, employeeIds, listIds, includeEmployees).ToArray();
      }
    }


    public EmployeeLoan AddOrUpdateEmployeeLoan(EmployeeLoan userItem) {
      using (var ent = ServiceManager.CreateModel()) {
        var dbItem = ent.EmployeeLoans.Query().SingleOrDefault(s => s.LoanID == userItem.LoanID);

        if (dbItem != null) {
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);

          ent.UpdateChildEntities(dbItem.Installments.ToArray(), userItem.Installments.ToArray(), (a, b) => a.InstallmentID == b.InstallmentID);
        } else
          ent.EmployeeLoans.Add(userItem);

        ent.SaveChanges();

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbItem == null ? SystemLogDataAction.Insert : SystemLogDataAction.Update, typeof(EmployeeLoan), userItem.LoanID.PackArray(),
          new SystemLogDataEntry() { EmployeeID = userItem.EmployeeID });
        ent.Logger.Flush();

        return ent.EmployeeLoans.Query().Single(s => s.LoanID == userItem.LoanID);
      }
    }

    public void DeleteLoan(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeLoan>(id);
      }
    }
    #endregion

  }

}

