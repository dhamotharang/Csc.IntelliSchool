using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeOfficialDocument[] GetEmployeeOfficialDocuments(Employee emp);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeOfficialDocumentSummary[] GetEmployeeOfficialDocumentSummary(DateTime month, int[] listIds, int[] employeeIds);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeOfficialDocumentType[] GetEmployeeOfficialDocumentTypes(Employee emp);
  }

}
