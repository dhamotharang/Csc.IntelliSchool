using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Branches
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeBranch[] GetBranches();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeBranch AddOrUpdateBranch(EmployeeBranch item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteBranch(int id);
    #endregion
  }

}