using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Bonuses
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeBonus AddOrUpdateBonus(EmployeeBonus item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteBonus(int id);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeBonus[] GetEmployeeBonusesByPeriod(int employeeId, DateTime month, PeriodFilter filter);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeBonus[] GetEmployeeBonuses(DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees);
    #endregion
  }

}