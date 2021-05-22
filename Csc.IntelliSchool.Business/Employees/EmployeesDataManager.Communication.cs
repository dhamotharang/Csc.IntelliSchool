
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    public static void GetContactNumbers(DateTime month, int[] listIds, int[] employeeIds, AsyncState<EmployeeContactDetails[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeContactDetails(month, listIds, employeeIds), callback);
    }
  }
}