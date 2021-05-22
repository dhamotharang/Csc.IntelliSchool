using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using System.IO;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {

    [OperationContract][ReferencePreservingDataContractFormat]
    Employee[] ApplyEmployeeMedicalSalaries(int[] employeeIds, bool applySalaries, DateTime? applyMonth);
    // TODO: Move to cleint
    [OperationContract][ReferencePreservingDataContractFormat]
    Stream GenerateEmployeeMedicalRequest(EmployeeMedicalRequest req);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalProgram[] GetEmployeeMedicalPrograms(EmployeeMedicalProgramDataFilter filter);
    [OperationContract][ReferencePreservingDataContractFormat]
    Employee[] GetMedicalEmployees(EmployeeDataFilter filter = EmployeeDataFilter.MedicalList, bool coveredOnly = false);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalProgramTemplate GetMedicalProgramTemplate(EmployeeMedicalRequest req);

  }

}
