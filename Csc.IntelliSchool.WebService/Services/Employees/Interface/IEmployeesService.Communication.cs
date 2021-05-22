using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeContactDetails[] GetEmployeeContactDetails(DateTime month, int[] listIds, int[] employeeIds);
  }

}
