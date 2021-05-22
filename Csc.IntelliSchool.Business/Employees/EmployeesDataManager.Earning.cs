
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    public static void GetEarningsSummary(DateTime startMonth, DateTime endMonth, int[] listIds, AsyncState<EmployeeEarningSummary[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeEarningsSummary(startMonth, endMonth, listIds), callback);
    }

    public static void GetSingleEarningsSummary(int employeeId, int year, PeriodFilter filter, AsyncState<SingleEmployeeEarningSummary[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetSingleEmployeeEarningsSummary(employeeId, year, filter), callback);
    }

    public static void GetEarnings(DateTime month, int employeeId, AsyncState<EmployeeEarning> callback) {
      GetEarnings(month, employeeId.PackArray(), (res, err) => Async.OnSingleCallback(res, err, callback));
    }
    public static void GetEarnings(DateTime month,  int[] employeeIds, AsyncState<EmployeeEarning[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeEarnings(month, null, employeeIds, EmploeeEarningCalculationMode.None), callback);
    }
    public static void GetListEarnings(DateTime month, int[] listIds, AsyncState<EmployeeEarning[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeEarnings(month, listIds, null, EmploeeEarningCalculationMode.None), callback);
    }

    public static void UpdateEarning(EmployeeEarning earning, AsyncState<EmployeeEarning> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.UpdateEmployeeEarning(earning), callback);
    }


    #region Recalculate
    public static void RecalculateEarnings(DateTime month, int employeeId, EmploeeEarningCalculationMode mode, AsyncState<EmployeeEarning> callback) {
      RecalculateEarnings(month, employeeId.PackArray(), mode, (res, err) => Async.OnSingleCallback(res, err, callback));
    }
    public static void RecalculateEarnings(DateTime month, int[] employeeIds, EmploeeEarningCalculationMode mode, AsyncState<EmployeeEarning[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeEarnings(month, null, employeeIds, mode), callback);
    }
    #endregion





  }
}