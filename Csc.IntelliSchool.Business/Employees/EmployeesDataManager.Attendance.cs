
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    public static void GetTerminalTransactions(string terminalIp, int userId, DateTime month, AsyncState<EmployeeTerminalTransaction[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeTerminalTransactions(terminalIp, userId, month), callback);
    }

    public static void GetAttendance(int employeeId, DateTime month, AsyncState<EmployeeAttendance[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetSingleEmployeeAttendance(employeeId, month, EmployeeRecalculateFlags.None), callback);
    }

    public static void GetAttendance(int? branchId, int? departmentId, int? positionId, int[] listIds, DateTime month, AsyncState<EmployeeAttendanceObject[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeAttendance
      (branchId, departmentId, positionId, listIds, month, 
        EmployeeRecalculateFlags.None), callback);
    }

    #region Update Item
    public static void UpdateAttendance(EmployeeAttendance att, AsyncState<EmployeeAttendance> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.UpdateEmployeeAttendance(att), callback);
    }
    #endregion


    #region Recalculate
    public static void RecalculateMonthlyEarning(DateTime month,  int[] listIds, EmployeeRecalculateFlags flags, AsyncState callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.RecalculateEmployeeMonthlyEarning(month, listIds, flags), callback);
    }

    public static void RecalculateAttendance(int employeeId, DateTime month, AsyncState<EmployeeAttendance[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetSingleEmployeeAttendance(employeeId, month, EmployeeRecalculateFlags.Attendance | EmployeeRecalculateFlags.EditedAttendance), callback);
    }
    #endregion

    #region Summary
    public static void GetAttendanceSummary(DateTime startMonth, DateTime endMonth, int[] listIds, AsyncState<EmployeeAttendanceSummary[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetAttendanceSummary(startMonth, endMonth, listIds), callback);
    }

    public static void GetEarningsAttendanceSummary(DateTime startMonth, DateTime endMonth, int[] listIds, AsyncState<EmployeeAttendanceSummary[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEarningsAttendanceSummary(startMonth, endMonth, listIds), callback);
    }

    #endregion
  }
}