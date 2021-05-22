using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Csc.Components.Common;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  /// <summary>
  /// Cumulative employee attendance/calculate operation state data.
  /// </summary>
  public class EmployeeCalculateAttendanceState : EmployeeCalculateState {
    public EmployeeShift[] Shifts { get; set; }
    public HashSet<EmployeeAttendance> Attendance { get; set; }
    public EmployeeTerminalTransaction[] Transactions { get; set; }
    public EmployeeTerminal[] Terminals { get; set; }
    public EmployeeTransactionRule[] TransactionRules { get; set; }
    public EmployeeVacation[] Vacations { get; set; }
    public EmployeeDepartmentVacation[] DepartmentVacations { get; set; }

    public EmployeeCalculateAttendanceState() : base() {

    }
    public EmployeeCalculateAttendanceState(DataEntities mdl)  :base(mdl) {
    }
  }
}