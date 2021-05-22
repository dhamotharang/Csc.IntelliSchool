using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Deductions
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDeduction AddOrUpdateDeduction(EmployeeDeduction item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteDeduction(int id);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDeduction[] GetEmployeeDeductionsByPeriod(int employeeId, DateTime month, PeriodFilter filter);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeDeduction[] GetEmployeeDeductions(DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees);
    #endregion
  }

}