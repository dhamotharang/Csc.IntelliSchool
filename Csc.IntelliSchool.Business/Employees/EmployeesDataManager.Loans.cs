
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Loans

    public static void GetLoans(DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployee, AsyncState<EmployeeLoan[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeLoans(startDate.ToMonth(), endDate.ToMonth(),employeeIds, listIds, includeEmployee), callback);
    }

    public static void GetLoans(int[] employeeIds, DateTime month, PeriodFilter filter, bool includeEmployee, AsyncState<EmployeeLoan[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeLoansByPeriod(employeeIds, month, filter, includeEmployee), callback);
    }
    public static void AddOrUpdateLoan(EmployeeLoanProxy proxy, AsyncState<EmployeeLoan> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateEmployeeLoan(EmployeeLoan.CreateObject(proxy)), callback);
    }


    public static void DeleteLoan(EmployeeLoan item, AsyncState<EmployeeLoan> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteLoan(item.LoanID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }
    #endregion

  }
}