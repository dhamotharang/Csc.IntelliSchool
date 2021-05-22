
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    public static void GetOfficialDocuments(Employee emp, AsyncState<EmployeeOfficialDocument[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeOfficialDocuments(emp), callback);
    }

    public static void GetOfficialDocumentTypes(Employee emp, AsyncState<EmployeeOfficialDocumentType[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeOfficialDocumentTypes(emp), callback);
    }

    public static void GetOfficialDocumentSummary(DateTime month, int[] listIds, int[] employeeIds, AsyncState<EmployeeOfficialDocumentSummary[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeOfficialDocumentSummary(month, listIds, employeeIds), callback);
    }
  }
}