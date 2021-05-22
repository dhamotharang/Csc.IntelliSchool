using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Dependants
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDependant AddOrUpdateEmployeeDependant(EmployeeDependant userItem);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteDependant(int id);
    #endregion
  }


}