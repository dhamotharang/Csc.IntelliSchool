using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Employees

    /*
     * Details
     * Salaries
     * Bank
     * Terminations
     */


    public static void GetEmployeeListDetails(DateTime month, int[] listIds, bool includeDependants, AsyncState<Employee[]> callback) {
      var filter = EmployeeIncludes.EmployeeListDetails;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMonthEmployees(filter, month, listIds, null), callback);
    }

    public static void GetEmployeeListSalaries(DateTime month, int[] listIds, bool includeDependants, AsyncState<Employee[]> callback) {
      var filter = EmployeeIncludes.EmployeeListSalaries;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMonthEmployees(filter, month, listIds, null), callback);
    }
    public static void GetEmployeeListBank(DateTime month, int[] listIds, bool includeDependants, AsyncState<Employee[]> callback) {
      var filter = EmployeeIncludes.EmployeeListBank;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMonthEmployees(filter, month, listIds, null), callback);
    }
    public static void GetTerminatedEmployees(DateTime? month, int[] listIds, bool includeDependants, AsyncState<Employee[]> callback) {
      var filter = EmployeeIncludes.EmployeeListFull;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetTerminatedEmployees(month, filter, listIds), callback);
    }





    public static void SelectEmployees(EmployeeSelectionType type, DateTime? month, int[] listIds, AsyncState<Employee[]> callback) {
      if (type == EmployeeSelectionType.Default) {
        Async.AsyncCall(() => Service.EmployeesService.Instance.GetMonthEmployees(EmployeeIncludes.Minimum, month.Value, listIds, null), callback);
      } else if (type == EmployeeSelectionType.Terminated) {
        Async.AsyncCall(() => Service.EmployeesService.Instance.GetTerminatedEmployees(month, EmployeeIncludes.Minimum, listIds), callback);
      } else if (type == EmployeeSelectionType.Medical)
        Async.AsyncCall(() => Service.EmployeesService.Instance.GetCoveredMedicalEmployees(EmployeeIncludes.MedicalMinimum), callback);
    }

    public static void GetSingleEmployee(int employeeId, DateTime month, bool includeDependants, AsyncState<Employee> callback) {
      var filter = EmployeeIncludes.EmployeeList;
      if (includeDependants)
        filter |= EmployeeIncludes.DependantList;

      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMonthEmployees(filter, month, null, new int[] { employeeId }),
        (res, err) => {
          if (callback == null)
            return;
          callback(res != null ? res.FirstOrDefault() : null, err);
        });
    }




    
    public static void AddOrUpdateEmployee(Employee item, AsyncState<Employee> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateEmployee(item), callback);
    }
    public static void TerminateEmployee(Employee item, AsyncState<Employee> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.TerminateEmployee(item), callback);
    }
    public static void ReenrollEmployee(Employee item, AsyncState<Employee> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.ReenrollEmployee(item), callback);
    }
    public static void CheckEmployeeTerminalUsed(int employeeId, int terminalId, int userId, AsyncState<bool> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.CheckEmployeeTerminalUsed(employeeId, terminalId, userId), callback);
    }
    public static void GetEmployeeSalaryUpdates(int employeeId, int year, PeriodFilter filter, AsyncState<EmployeeSalaryUpdate[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeSalaryUpdates(employeeId, year, filter), callback);
    }


    #endregion

    
  }
}