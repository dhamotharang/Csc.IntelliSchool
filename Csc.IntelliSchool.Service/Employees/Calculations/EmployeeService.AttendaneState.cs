using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    private static void InternalLoadCalculateAttendanceState(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state) {
      ////////////////////////
      // Flags
      if (state.Flags == null)
        state.Flags = SystemService.InternalGetHumanResourcesFlagList(state.Model);

      InternalLoadCalculateStateShifts(employees, options, state);
      InternalLoadCalculateStateTransactions(employees, options, state);
      InternalLoadCalculateStateTransactionRules(employees, state);
      InternalLoadCalculateStateVacations(employees, options, state);
      InternalLoadCalculateStateDepartmentVacations(employees, options, state);
      InternalLoadCalculateStateAttendance(employees, options, state);
    }

    private static void InternalLoadCalculateStateShifts(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state) {
      if (state.Shifts != null)
        return;

      var shiftIds = employees.Where(s => s.ShiftID != null).Select(s => s.ShiftID.Value).Distinct().ToArray();

      state.Shifts = state.Model.EmployeeShiftOverrides.Query(EmployeeShiftOverrideIncludes.Shift, new EmployeeShiftOverrideDataCriteria() {
        ShiftIDs = shiftIds,
        StartDate = options.StartDate,
        EndDate = options.EndDate,
        Active = true
      }).OrderBy(s => s.StartDate).ThenBy(s => s.OverrideID).ToArray().Select(s => s.Shift).ToArray();

      shiftIds = shiftIds.Except(state.Shifts.Select(s => s.ShiftID)).ToArray();
      state.Shifts = state.Shifts.Concat(state.Model.EmployeeShifts.Query().Where(s => shiftIds.Contains(s.ShiftID)).ToArray()).ToArray();

    }

    private static void InternalLoadCalculateStateTransactions(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state) {
      if (state.Transactions != null)
        return;

      var terminalEmployees = employees.Where(s => s.IsTerminalUser).ToArray();
      var terminalIds = terminalEmployees.Select(s => s.TerminalID.Value).Distinct().ToArray();

      if (terminalIds.Count() > 0) {
        state.Terminals = state.Model.EmployeeTerminals.Where(s => terminalIds.Contains(s.TerminalID)).ToArray();

        var terminalIPs = state.Terminals.Select(s => s.IP).ToArray();
        var terminalUserIDs = terminalEmployees.Select(s => s.TerminalUserID).Distinct().ToArray();

        var qry = state.Model.EmployeeTerminalTransactions.Where(s => s.DateTime >= options.StartDate && s.DateTime <= options.EndTime).AsQueryable();

        // trick to improve query performance and limit returned results
        if (terminalIPs.Count() == 1 && terminalUserIDs.Count() == 1) {
          var tmpTerminalIp = terminalIPs.First();
          var tmpUserId = terminalUserIDs.First();
          state.Transactions = qry.Where(s => s.TerminalIP == tmpTerminalIp && s.UserID == tmpUserId).ToArray();
        } else // did you find better way?
          state.Transactions = qry.Where(s => terminalIPs.Contains(s.TerminalIP) || terminalUserIDs.Contains(s.UserID)).ToArray();

      } else {
        state.Transactions = new EmployeeTerminalTransaction[] { };
      }
    }
    private static void InternalLoadCalculateStateTransactionRules(Employee[] employees, EmployeeCalculateAttendanceState state) {
      if (state.TransactionRules != null)
        return;

      if (employees.Any(s => s.IsTerminalUser && s.ShiftID != null))
        state.TransactionRules = state.Model.EmployeeTransactionRules.OrderBy(s => s.Type).ThenBy(s => s.Time).ThenBy(s => s.RuleID).ToArray();
      else
        state.TransactionRules = new EmployeeTransactionRule[] { };
    }
    private static void InternalLoadCalculateStateVacations(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state) {
      if (state.Vacations != null)
        return;

      state.Vacations = state.Model.EmployeeVacations.Query(
        new EmployeeDataCriteria() {
          EmployeeIDs = employees.Select(s => s.EmployeeID).ToArray(),
          StartDate = options.StartDate, EndDate = options.EndDate
        }).ToArray();
    }
    private static void InternalLoadCalculateStateDepartmentVacations(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state) {
      if (state.DepartmentVacations != null)
        return;

      int[] departmentIds = employees.Where(s => s.DepartmentID != null).Select(s => s.DepartmentID.Value).Distinct().ToArray();

      if (departmentIds.Count() > 0)
        state.DepartmentVacations = state.Model.EmployeeDepartmentVacations.Query(EmployeeDepartmentVacationIncludes.None,
            new EmployeeDepartmentVacationDataCriteria() { DepartmentIDs = departmentIds, StartDate = options.StartDate, EndDate = options.EndDate }).ToArray();
      else
        state.DepartmentVacations = new EmployeeDepartmentVacation[] { };
    }
    private static void InternalLoadCalculateStateAttendance(Employee[] employees, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState state) {
      if (state.Attendance != null)
        return;

      state.Attendance = state.Model.EmployeeAttendance.Query(EmployeeAttendanceIncludes.TimeOffs,
        new EmployeeRangeDataCriteria() {
          EmployeeIDs = employees.Select(s => s.EmployeeID).ToArray(),
          StartDate = options.StartDate,
          EndDate = options.EndDate
        }).OrderBy(s => s.Date).ToHashSet();
    }

    private static EmployeeCalculateAttendanceData InternalCreateEmployeeCalculateAttendanceData(Employee emp, EmployeeCalculateOptions options, EmployeeCalculateAttendanceState operationState) {
      EmployeeCalculateAttendanceData data = new EmployeeCalculateAttendanceData(emp);

      //DayOfWeek[] weekends = emp.Shift != null ? emp.Shift.GetWeekends() : new DayOfWeek[] { };

      EmployeeTerminal terminal = null;
      if (emp.IsTerminalUser)
        terminal = operationState.Terminals.SingleOrDefault(s => s.TerminalID == emp.TerminalID);
      data.Transactions = terminal != null ?
        operationState.Transactions.Where(s => s.TerminalIP == terminal.IP && s.UserID == emp.TerminalUserID).ToArray() :
        new EmployeeTerminalTransaction[] { };

      var vacations = operationState.Vacations.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      data.UnpaidVacationDays =
        vacations.Where(s => s.IsPaid == false).SelectMany(s => s.GetDays())
        .Where(s => s.Date >= options.StartDate && s.Date <= options.EndDate).Distinct().ToArray();
      data.PaidVacationDays =
        vacations.Where(s => s.IsPaid == true).SelectMany(s => s.GetDays())
        .Concat((emp.DepartmentID != null ? operationState.DepartmentVacations : new EmployeeDepartmentVacation[] { })
        .Where(s => s.Departments.Where(x => x.DepartmentID == emp.DepartmentID).Count() > 0).SelectMany(s => s.GetDays()))
        .Where(s => s.Date >= options.StartDate && s.Date <= options.EndDate).Distinct().ToArray();
      data.PaidVacationDays = data.PaidVacationDays.Except(data.UnpaidVacationDays).ToArray();

      data.Attendance = operationState.Attendance.Where(s => s.EmployeeID == emp.EmployeeID).ToHashSet();

      if (data.Employee.ShiftID != null)
        data.Shift = operationState.Shifts.SingleOrDefault(a => a.ShiftID == data.Employee.ShiftID);

      return data;
    }
  }

}