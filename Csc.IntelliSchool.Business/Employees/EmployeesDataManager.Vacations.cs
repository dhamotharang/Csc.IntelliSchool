
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeVacationType[] VacationTypes {
      get { return DataManager.Cache.Get<EmployeeVacationType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeVacationType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }

    #region Vacations
    public static void AddOrUpdateVacation(EmployeeVacation item, AsyncState<EmployeeVacation> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateVacation(item), callback);
    }

    public static void DeleteVacation(EmployeeVacation item, AsyncState<EmployeeVacation> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteVacation(item.VacationID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }

    public static void GetVacations(int employeeId, DateTime month, PeriodFilter filter, AsyncState<EmployeeVacation[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeVacationsByPeriod(employeeId, month, filter), callback);
    }
    public static void GetVacations(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployee, AsyncState<EmployeeVacation[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeVacations(typeIds, startDate, endDate, employeeIds, listIds, includeEmployee), callback);
    }
    #endregion

    #region VacationTypes
    public static void GetVacationTypes(bool forceLoad, AsyncState<EmployeeVacationType[]> callback) {
      Action<IEnumerable<EmployeeVacationType>, Exception> locCallback = (res, err) => {
        if (res != null) {
          res = res.OrderBy(s => s.Name);
        }
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && VacationTypes != null) {
        locCallback(VacationTypes, null);
        return;
      }

      VacationTypes = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetVacationTypes(), (res, err) => {
        if (err == null)
          VacationTypes = res.OrderBy(s => s.Name).ToArray();
        locCallback(VacationTypes, err);
      });
    }

    public static void DeleteVacationType(EmployeeVacationType item, AsyncState<EmployeeVacationType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteVacationType(item.TypeID), (err) => {
        if (err == null)
          VacationTypes = VacationTypes.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }

    public static void IsVacationTypeUsed(int id, AsyncState<bool> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.IsVacationTypeUsed(id), callback);
    }


    public static void AddVacationType(EmployeeVacationType item, AsyncState<EmployeeVacationType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateVacationType(item), (res, err) => {
        if (err == null)
          VacationTypes = VacationTypes.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }

    public static void UpdateVacationType(EmployeeVacationType item, AsyncState<EmployeeVacationType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateVacationType(item), (res, err) => {
        if (err == null) {
          VacationTypes = VacationTypes.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}