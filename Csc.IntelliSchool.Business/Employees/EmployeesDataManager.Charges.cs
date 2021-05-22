
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeChargeType[] ChargeTypes {
      get { return DataManager.Cache.Get<EmployeeChargeType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeChargeType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }

    #region Charges
    public static void AddOrUpdateCharge(EmployeeCharge item, AsyncState<EmployeeCharge> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateCharge(item), callback);
    }

    public static void DeleteCharge(EmployeeCharge item, AsyncState<EmployeeCharge> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteCharge(item.ChargeID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }

    public static void GetCharges(int employeeId, DateTime month, PeriodFilter filter, AsyncState<EmployeeCharge[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeChargesByPeriod(employeeId, month, filter), callback);
    }
    public static void GetCharges(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployee, AsyncState<EmployeeCharge[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeCharges(typeIds, startDate, endDate, employeeIds, listIds, includeEmployee), callback);
    }
    #endregion

    #region ChargeTypes
    public static void GetChargeTypes(bool forceLoad, AsyncState<EmployeeChargeType[]> callback) {
      Action<IEnumerable<EmployeeChargeType>, Exception> locCallback = (res, err) => {
        if (res != null) {
          res = res.OrderBy(s => s.Name);
        }
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && ChargeTypes != null) {
        locCallback(ChargeTypes, null);
        return;
      }

      ChargeTypes = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetChargeTypes(), (res, err) => {
        if (err == null)
          ChargeTypes = res.OrderBy(s => s.Name).ToArray();
        locCallback(ChargeTypes, err);
      });
    }

    public static void DeleteChargeType(EmployeeChargeType item, AsyncState<EmployeeChargeType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteChargeType(item.TypeID), (err) => {
        if (err == null)
          ChargeTypes = ChargeTypes.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void IsChargeTypeUsed(int id, AsyncState<bool> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.IsChargeTypeUsed(id), callback);
    }


    public static void AddChargeType(EmployeeChargeType item, AsyncState<EmployeeChargeType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateChargeType(item), (res, err) => {
        if (err == null)
          ChargeTypes = ChargeTypes.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateChargeType(EmployeeChargeType item, AsyncState<EmployeeChargeType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateChargeType(item), (res, err) => {
        if (err == null) {
          ChargeTypes = ChargeTypes.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}