using Csc.IntelliSchool.Data;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Bonuses
    [OperationContract][ReferencePreservingDataContractFormat]
    
    EmployeeVacation AddOrUpdateVacation(EmployeeVacation item);
    [OperationContract][ReferencePreservingDataContractFormat]
    
    void DeleteVacation(int id);
    #endregion
  }
}