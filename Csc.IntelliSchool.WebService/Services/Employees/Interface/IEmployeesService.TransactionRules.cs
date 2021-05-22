using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region TransactionRules

    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeTransactionRule[] GetTransactionRules();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeTransactionRule AddOrUpdateTransactionRule(EmployeeTransactionRule item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteTransactionRule(int id);
    #endregion
  }

}