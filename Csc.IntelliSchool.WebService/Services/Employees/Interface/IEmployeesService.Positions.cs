using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Positions
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeePosition[] GetPositions();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeePosition AddOrUpdatePosition(EmployeePosition item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeletePosition(int id);
    #endregion
  }

}