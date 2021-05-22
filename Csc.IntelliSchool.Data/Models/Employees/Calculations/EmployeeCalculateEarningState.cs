using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Csc.Components.Common;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  public class EmployeeCalculateEarningState : EmployeeCalculateState {
    public EmployeeSalary[] Salaries { get; set; }
    public EmployeeAllowance[] Allowances { get; set; }
    public EmployeeCharge[] Charges { get; set; }
    public EmployeeBonus[] Bonuses { get; set; }
    public EmployeeDeduction[] Deductions { get; set; }
    public EmployeeLoan[] Loans { get; set; }

    public IEnumerable<EmployeeAttendance> Attendance { get; set; }
    public HashSet<EmployeeEarning> Earnings { get; set; }

    public EmployeeCalculateEarningState() : base() {

    }
    public EmployeeCalculateEarningState(DataEntities mdl) : base(mdl) {
    }
  }
}