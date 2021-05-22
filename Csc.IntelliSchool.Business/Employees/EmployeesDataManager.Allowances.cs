
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeAllowanceType[] AllowanceTypes {
      get { return DataManager.Cache.Get<EmployeeAllowanceType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeAllowanceType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }

    #region Allowances
    public static void AddOrUpdateAllowance(EmployeeAllowance item, AsyncState<EmployeeAllowance> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateAllowance(item), callback);
    }

    public static void DeleteAllowance(EmployeeAllowance item, AsyncState<EmployeeAllowance> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteAllowance(item.AllowanceID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }

    public static void GetAllowances(int employeeId, DateTime month, PeriodFilter filter, AsyncState<EmployeeAllowance[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeAllowancesByPeriod(employeeId, month, filter), callback);
    }
    public static void GetAllowances(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployee, AsyncState<EmployeeAllowance[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeAllowances(typeIds, startDate, endDate, employeeIds, listIds, includeEmployee), callback);
    }
    #endregion

    #region AllowanceTypes
    public static void GetAllowanceTypes(bool forceLoad, AsyncState<EmployeeAllowanceType[]> callback) {
      Action<IEnumerable<EmployeeAllowanceType>, Exception> locCallback = (res, err) => {
        if (res != null) {
          res = res.OrderBy(s => s.Name);
        }
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && AllowanceTypes != null) {
        locCallback(AllowanceTypes, null);
        return;
      }

      AllowanceTypes = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetAllowanceTypes(), (res, err) => {
        if (err == null)
          AllowanceTypes = res.OrderBy(s => s.Name).ToArray();
        locCallback(AllowanceTypes, err);
      });
    }

    public static void DeleteAllowanceType(EmployeeAllowanceType item, AsyncState<EmployeeAllowanceType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteAllowanceType(item.TypeID), (err) => {
        if (err == null)
          AllowanceTypes = AllowanceTypes.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void IsAllowanceTypeUsed(int id, AsyncState<bool> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.IsAllowanceTypeUsed(id), callback);
    }


    public static void AddAllowanceType(EmployeeAllowanceType item, AsyncState<EmployeeAllowanceType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateAllowanceType(item), (res, err) => {
        if (err == null)
          AllowanceTypes = AllowanceTypes.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateAllowanceType(EmployeeAllowanceType item, AsyncState<EmployeeAllowanceType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateAllowanceType(item), (res, err) => {
        if (err == null) {
          AllowanceTypes = AllowanceTypes.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}