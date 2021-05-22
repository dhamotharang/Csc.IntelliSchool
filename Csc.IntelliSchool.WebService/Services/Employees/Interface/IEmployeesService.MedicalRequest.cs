using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using System.IO;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeMedicalRequestType[] GetMedicalRequestTypes();
  }

}
