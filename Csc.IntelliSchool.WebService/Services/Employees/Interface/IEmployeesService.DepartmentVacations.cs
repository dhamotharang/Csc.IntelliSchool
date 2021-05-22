using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {

    #region DepartmentVacations
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDepartmentVacation AddOrUpdateEmployeeDepartmentVacation(EmployeeDepartmentVacation userItem);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteDepartmentVacation(int id);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeVacation[] GetEmployeeVacations(int employeeId, DateTime month, PeriodFilter filter);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDepartmentVacation[] GetEmployeeDepartmentVacations(int departmentId, DateTime month, PeriodFilter filter);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDepartmentVacation[] GetEmployeeDepartmentVacationsByYear(int year);
    #endregion
  }

}