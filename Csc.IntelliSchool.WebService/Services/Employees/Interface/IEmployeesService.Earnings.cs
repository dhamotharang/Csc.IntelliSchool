using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeEarning[] GetEmployeeEarnings(DateTime month, int[] listIds, int[] employeeIds, EmploeeEarningCalculationMode mode);
    [OperationContract][ReferencePreservingDataContractFormat]
    SingleEmployeeEarningSummary[] GetSingleEmployeeEarningsSummary(int employeeId, int year, PeriodFilter filter);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeEarningSummary[] GetEmployeeEarningsSummary(DateTime startMonth, DateTime endMonth, int[] listIds);
    [OperationContract][ReferencePreservingDataContractFormat]
    void RecalculateEmployeeEarnings(DateTime month, int[] employeeIds, EmployeeRecalculateFlags optionFlags);
    [OperationContract][ReferencePreservingDataContractFormat]
    void RecalculateEmployeeMonthly(DateTime month, int[] listIds, EmployeeRecalculateFlags flags);
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeEarning UpdateEmployeeEarning(EmployeeEarning earning);
  }

}
