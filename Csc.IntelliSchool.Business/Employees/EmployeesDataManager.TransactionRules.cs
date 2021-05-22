
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region TransactionRules
    public static void AddOrUpdateTransactionRule(EmployeeTransactionRule item, AsyncState<EmployeeTransactionRule> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateTransactionRule(item), callback);
    }

    public static void DeleteTransactionRule(EmployeeTransactionRule item, AsyncState<EmployeeTransactionRule> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteTransactionRule(item.RuleID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }


    public static void GetTransactionRules(AsyncState<EmployeeTransactionRule[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetTransactionRules(), callback);
    }
    #endregion

  }
}