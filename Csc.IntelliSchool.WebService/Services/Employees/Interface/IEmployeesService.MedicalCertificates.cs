using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using System.IO;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {

    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalCertificate AddOrUpdateEmployeeMedicalCertificate(EmployeeMedicalCertificate userItem);
    [OperationContract][ReferencePreservingDataContractFormat]
    bool CheckEmployeeMedicalCodeUsed(int[] certificateIds, int providerId, string code);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalCertificateGroup[] GenerateEmployeeMedicalCertificates(int[] employeeIds, int[] programIds);
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee[] GenerateEmployeeMedicalProgramCertificates(int[] programIds, int[] employeeIds);
    [OperationContract][ReferencePreservingDataContractFormat]
    void RecalculateEmployeeMedicalCertificates(int[] certificateIds, bool recalcRates, bool recalcCertificates, bool excludeCustomCertificates);

    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteEmployeeMedicalCertificate(int id);
  }

}
