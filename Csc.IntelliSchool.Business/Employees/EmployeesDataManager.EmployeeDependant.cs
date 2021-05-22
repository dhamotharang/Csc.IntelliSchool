
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Dependants

    public static void AddOrUpdateDependant(EmployeeDependant item, AsyncState<EmployeeDependant> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateEmployeeDependant(item), callback);
    }


    public static void DeleteDependant(EmployeeDependant item, AsyncState<EmployeeDependant> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteDependant(item.DependantID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }
    #endregion

  }
}