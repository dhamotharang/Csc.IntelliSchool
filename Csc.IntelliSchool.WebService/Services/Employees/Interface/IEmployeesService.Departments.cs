using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Departments
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDepartment[] GetDepartments();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDepartment AddOrUpdateDepartment(EmployeeDepartment item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteDepartment(int id);
    #endregion
  }

}