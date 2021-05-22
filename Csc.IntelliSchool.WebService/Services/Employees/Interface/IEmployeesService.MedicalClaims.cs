using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using System.IO;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalClaim AddOrUpdateEmployeeMedicalClaim(EmployeeMedicalClaim userItem);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalClaim[] GetEmployeeMedicalClaims(int[] statusIds, DateTime? startDate, DateTime? endDate);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteEmployeeMedicalClaim(int id);


    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalClaimStatus[] GetMedicalClaimStatuses();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalClaimType[] GetMedicalClaimTypes();

  }

}
