using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Terminals

    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeTerminal[] GetTerminals();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeTerminal AddOrUpdateTerminal(EmployeeTerminal item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteTerminal(int id);
    #endregion
  }

}