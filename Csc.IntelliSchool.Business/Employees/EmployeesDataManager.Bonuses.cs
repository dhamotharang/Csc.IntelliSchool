
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeBonusType[] BonusTypes {
      get { return DataManager.Cache.Get<EmployeeBonusType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeBonusType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }

    #region Bonuses
    public static void AddOrUpdateBonus(EmployeeBonus item, AsyncState<EmployeeBonus> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateBonus(item), callback);
    }

    public static void DeleteBonus(EmployeeBonus item, AsyncState<EmployeeBonus> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteBonus(item.BonusID), (err) => Async.OnCallback(err == null ? item : null, err, callback) );
    }

    public static void GetBonuses(int employeeId, DateTime month, PeriodFilter filter, AsyncState<EmployeeBonus[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeBonusesByPeriod(employeeId, month, filter), callback);
    }
    public static void GetBonuses(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployee, AsyncState<EmployeeBonus[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeBonuses(typeIds, startDate, endDate, employeeIds, listIds, includeEmployee), callback);
    }
    #endregion

    #region BonusTypes
    public static void GetBonusTypes(bool forceLoad, AsyncState<EmployeeBonusType[]> callback) {
      Action<IEnumerable<EmployeeBonusType>, Exception> locCallback = (res, err) => {
        if (res != null) {
          res = res.OrderBy(s => s.Name);
        }
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && BonusTypes != null) {
        locCallback(BonusTypes, null);
        return;
      }

      BonusTypes = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetBonusTypes(), (res, err) => {
        if (err == null)
          BonusTypes = res.OrderBy(s => s.Name).ToArray();
        locCallback(BonusTypes, err);
      });
    }

    public static void DeleteBonusType(EmployeeBonusType item, AsyncState<EmployeeBonusType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteBonusType(item.TypeID), (err) => {
        if (err == null)
          BonusTypes = BonusTypes.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void IsBonusTypeUsed(int id, AsyncState<bool> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.IsBonusTypeUsed(id), callback);
    }


    public static void AddBonusType(EmployeeBonusType item, AsyncState<EmployeeBonusType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateBonusType(item), (res, err) => {
        if (err == null)
          BonusTypes = BonusTypes.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateBonusType(EmployeeBonusType item, AsyncState<EmployeeBonusType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateBonusType(item), (res, err) => {
        if (err == null) {
          BonusTypes = BonusTypes.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}