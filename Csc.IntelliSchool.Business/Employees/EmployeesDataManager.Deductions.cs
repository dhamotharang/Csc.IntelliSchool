
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeDeductionType[] DeductionTypes {
      get { return DataManager.Cache.Get<EmployeeDeductionType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeDeductionType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }

    #region Deductions
    public static void AddOrUpdateDeduction(EmployeeDeduction item, AsyncState<EmployeeDeduction> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateDeduction(item), callback);
    }

    public static void DeleteDeduction(EmployeeDeduction item, AsyncState<EmployeeDeduction> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteDeduction(item.DeductionID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }

    public static void GetDeductions(int employeeId, DateTime month, PeriodFilter filter, AsyncState<EmployeeDeduction[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeDeductionsByPeriod(employeeId, month, filter), callback);
    }
    public static void GetDeductions(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployee, AsyncState<EmployeeDeduction[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeDeductions(typeIds, startDate, endDate, employeeIds, listIds, includeEmployee), callback);
    }
    #endregion

    #region DeductionTypes
    public static void GetDeductionTypes(bool forceLoad, AsyncState<EmployeeDeductionType[]> callback) {
      Action<IEnumerable<EmployeeDeductionType>, Exception> locCallback = (res, err) => {
        if (res != null) {
          res = res.OrderBy(s => s.Name);
        }
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && DeductionTypes != null) {
        locCallback(DeductionTypes, null);
        return;
      }

      DeductionTypes = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetDeductionTypes(), (res, err) => {
        if (err == null)
          DeductionTypes = res.OrderBy(s => s.Name).ToArray();
        locCallback(DeductionTypes, err);
      });
    }

    public static void DeleteDeductionType(EmployeeDeductionType item, AsyncState<EmployeeDeductionType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteDeductionType(item.TypeID), (err) => {
        if (err == null)
          DeductionTypes = DeductionTypes.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void IsDeductionTypeUsed(int id, AsyncState<bool> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.IsDeductionTypeUsed(id), callback);
    }


    public static void AddDeductionType(EmployeeDeductionType item, AsyncState<EmployeeDeductionType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateDeductionType(item), (res, err) => {
        if (err == null)
          DeductionTypes = DeductionTypes.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateDeductionType(EmployeeDeductionType item, AsyncState<EmployeeDeductionType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateDeductionType(item), (res, err) => {
        if (err == null) {
          DeductionTypes = DeductionTypes.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}