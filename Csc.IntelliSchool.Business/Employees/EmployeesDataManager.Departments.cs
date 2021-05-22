
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeDepartment[] Departments {
      get { return DataManager.Cache.Get<EmployeeDepartment[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeDepartment[]>();
        else
          DataManager.Cache.Add(value);
      }
    }


    #region Departments
    public static void GetDepartments(bool forceLoad, int? listId, AsyncState<EmployeeDepartment[]> callback) {
      Action<IEnumerable<EmployeeDepartment>, Exception> locCallback = (res, err) => {
        Async.OnCallback(res != null ?
        res.Where(s => s.Lists.Any(x => x.ListID == listId)).OrderBy(s => s.Name).ToArray()
          .Concat(res.Where(s => s.Lists.Any(x => x.ListID == listId) == false).OrderBy(s => s.Name).ToArray())
          .ToArray()
          : null, err, callback);
      };

      if (forceLoad == false && Departments != null) {
        locCallback(Departments, null);
        return;
      }

      Departments = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetDepartments(), (res, err) => {
        if (err == null)
          Departments = res.OrderBy(s => s.Name).ToArray();
        locCallback(Departments, err);
      });
    }

    public static void DeleteDepartment(EmployeeDepartment item, AsyncState<EmployeeDepartment> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteDepartment(item.DepartmentID), (err) => {
        if (err == null)
          Departments = Departments.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void AddDepartment(EmployeeDepartment item, AsyncState<EmployeeDepartment> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateDepartment(item), (res, err) => {
        if (err == null)
          Departments = Departments.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateDepartment(EmployeeDepartment item, AsyncState<EmployeeDepartment> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateDepartment(item), (res, err) => {
        if (err == null) {
          Departments = Departments.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }

    #endregion

  }
}