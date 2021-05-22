
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Vacations


    public static void AddOrUpdateDepartmentVacations(EmployeeDepartmentVacation item, AsyncState<EmployeeDepartmentVacation> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateEmployeeDepartmentVacation(item), callback);
    }


    public static void GetDepartmentVacations(int year, AsyncState<EmployeeDepartmentVacation[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeDepartmentVacationsByYear(year), callback);
    }
    public static void GetDepartmentVacations(int departmentId, DateTime month, PeriodFilter filter, AsyncState<EmployeeDepartmentVacation[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeDepartmentVacations(departmentId, month, filter), callback);
    }



    public static void DeleteDepartmentVacation(EmployeeDepartmentVacation item, AsyncState<EmployeeDepartmentVacation> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteDepartmentVacation(item.VacationID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }
    #endregion
  }
}