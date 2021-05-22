using Csc.IntelliSchool.Data;
using System.Linq;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    private static void InternalLoadCalculateEarningState(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Flags == null)
        state.Flags = SystemService.InternalGetHumanResourcesFlagList(state.Model);

      int[] employeeIds = employees.Select(s => s.EmployeeID).ToArray();


      InternalLoadCalculateStateSalaries(employeeIds, options, state);
      InternalLoadCalculateStateAllowances(employeeIds, options, state);
      InternalLoadCalculateStateCharges(employeeIds, options, state);
      InternalLoadCalculateStateBonuses(employeeIds, options, state);
      InternalLoadCalculateStateDeductions(employeeIds, options, state);
      InternalLoadCalculateStateLoans(employeeIds, options, state);
    }


    private static void InternalLoadCalculateStateSalaries(int[] employeeIds, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Salaries != null)
        return;

      state.Salaries = state.Model.EmployeeSalaries.Where(s => employeeIds.Contains(s.EmployeeID)).ToArray();
    }
    private static void InternalLoadCalculateStateAllowances(int[] employeeIds, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Allowances != null)
        return;

      state.Allowances = state.Model.EmployeeAllowances.Query(new EmployeeDataCriteria() {
        EmployeeIDs = employeeIds,
        StartDate = options.StartDate,
        EndDate = options.EndDate
      }).ToArray();
    }
    private static void InternalLoadCalculateStateCharges(int[] employeeIds, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Charges != null)
        return;

      state.Charges = state.Model.EmployeeCharges.Query(new EmployeeDataCriteria() {
        EmployeeIDs = employeeIds,
        StartDate = options.StartDate,
        EndDate = options.EndDate
      }).ToArray();
    }
    private static void InternalLoadCalculateStateBonuses(int[] employeeIds, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Bonuses != null)
        return;

      state.Bonuses = state.Model.EmployeeBonuses.Query(new EmployeeDataCriteria() {
        EmployeeIDs = employeeIds,
        StartDate = options.StartDate,
        EndDate = options.EndDate
      }).Where(s => s.IncludeInSalary).ToArray();
    }
    private static void InternalLoadCalculateStateDeductions(int[] employeeIds, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Deductions != null)
        return;

      state.Deductions = state.Model.EmployeeDeductions.Query(new EmployeeDataCriteria() {
        EmployeeIDs = employeeIds,
        StartDate = options.StartDate,
        EndDate = options.EndDate
      }).ToArray();
    }
    private static void InternalLoadCalculateStateLoans(int[] employeeIds, EmployeeCalculateOptions options, EmployeeCalculateEarningState state) {
      if (state.Loans != null)
        return;

      state.Loans = state.Model.EmployeeLoanInstallments.Query(EmployeeLoanInstallmentIncludes.Loan, new EmployeeLoanDataCriteria() {
        EmployeeIDs = employeeIds
      }.SetMonth(options.Month).As<EmployeeLoanDataCriteria>()).ToArray().Select(s => s.Loan).ToArray();
    }

    private static EmployeeCalculateEarningData InternalCreateEmployeeCalculateEarningData(Employee emp, EmployeeCalculateOptions options, EmployeeCalculateEarningState operationState) {
      EmployeeCalculateEarningData data = new EmployeeCalculateEarningData(emp);

      data.Salary = operationState.Salaries.SingleOrDefault(s => s.EmployeeID == emp.EmployeeID);
      data.Earning = operationState.Earnings.SingleOrDefault(s => s.EmployeeID == emp.EmployeeID);
      data.Attendance = operationState.Attendance.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      data.Allowances = operationState.Allowances.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      data.Charges = operationState.Charges.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      data.Bonuses = operationState.Bonuses.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      data.Deductions = operationState.Deductions.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      data.Loans = operationState.Loans.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();

      return data;
    }
  }
}