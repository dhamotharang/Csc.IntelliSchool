
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeBranch[] Branches {
      get { return DataManager.Cache.Get<EmployeeBranch[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeBranch[]>();
        else
          DataManager.Cache.Add(value);
      }
    }


    #region Branches
    public static void GetBranches(bool forceLoad, AsyncState<EmployeeBranch[]> callback) {
      Action<IEnumerable<EmployeeBranch>, Exception> locCallback = (res, err) => {
        if (res != null) {
          res = res.OrderBy(s => s.Name);
        }
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && Branches != null) {
        locCallback(Branches, null);
        return;
      }

      Branches = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetBranches(), (res, err) => {
        if (err == null)
          Branches = res.OrderBy(s => s.Name).ToArray();
        locCallback(Branches, err);
      });
    }

    public static void DeleteBranch(EmployeeBranch item, AsyncState<EmployeeBranch> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteBranch(item.BranchID), (err) => {
        if (err == null)
          Branches = Branches.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void AddBranch(EmployeeBranch item, AsyncState<EmployeeBranch> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateBranch(item), (res, err) => {
        if (err == null)
          Branches = Branches.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateBranch(EmployeeBranch item, AsyncState<EmployeeBranch> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateBranch(item), (res, err) => {
        if (err == null) {
          Branches = Branches.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}