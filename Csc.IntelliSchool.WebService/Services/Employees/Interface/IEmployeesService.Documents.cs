using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {

    #region Documents
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDocument AddOrUpdateEmployeeDocument(EmployeeDocument doc, string fileUpload);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDocument[] GetEmployeeDocuments(int employeeId);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteDocument(int id);
    #endregion
  }

}