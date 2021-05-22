using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Loans
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeLoan[] GetEmployeeLoansByPeriod(int[] employeeIds, DateTime month, PeriodFilter filter, bool includeEmployee);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeLoan[] GetEmployeeLoans(DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeLoan AddOrUpdateEmployeeLoan(EmployeeLoan userItem);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteLoan(int id);
    #endregion
  }


}