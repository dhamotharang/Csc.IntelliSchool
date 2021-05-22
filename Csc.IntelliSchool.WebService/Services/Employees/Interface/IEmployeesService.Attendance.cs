using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeAttendance[] GetSingleEmployeeAttendance(int employeeId, DateTime month, EmployeeRecalculateFlags flags);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeAttendanceObject[] GetEmployeeAttendance(int? branchId, int? departmentId, int? positionId, int[] listIds, DateTime month, EmployeeRecalculateFlags flags);
    [OperationContract][ReferencePreservingDataContractFormat]
    void RecalculateEmployeeAttendance(DateTime month, EmployeeRecalculateFlags flags);
    [OperationContract][ReferencePreservingDataContractFormat]
    void RecalculateEmployeeAttendanceByEmployees(DateTime month, int[] employeeIds, EmployeeRecalculateFlags flags);
    [OperationContract][ReferencePreservingDataContractFormat]
    void RecalculateEmployeeAttendanceByTerminal(DateTime month, int terminalId, EmployeeRecalculateFlags flags);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeAttendance UpdateEmployeeAttendance(EmployeeAttendance userItem);
  }

}
