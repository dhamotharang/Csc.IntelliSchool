using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  [ServiceContract]
  public partial interface IEmployeesService {
    #region Employees
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee[] GetCurrentEmployees(EmployeeDataFilter filter, DateTime month, int[] listIds, int[] employeeIds);
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee[] GetTerminatedEmployees(DateTime? month, EmployeeDataFilter filter, int[] listIds);
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee AddOrUpdateEmployee(Employee userItem);
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee TerminateEmployee(Employee item);
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee ReenrollEmployee(Employee item);
    [OperationContract][ReferencePreservingDataContractFormat]
    bool CheckEmployeeTerminalUsed(int employeeId, int terminalId, int userId);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeSalaryUpdate[] GetEmployeeSalaryUpdates(int employeeId, int year, PeriodFilter filter);
    #endregion

 
  }

}