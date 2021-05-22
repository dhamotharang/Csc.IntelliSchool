using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  /// <summary>
  /// Filtered employee data used for attendance and earning calculations.
  /// </summary>
  public class EmployeeCalculateEarningData : EmployeeCalculateData {
    public EmployeeSalary Salary { get; set; }
    public EmployeeEarning Earning { get; set; }
    public EmployeeAttendance[] Attendance { get; set; }

    public EmployeeAllowance[] Allowances { get; set; }
    public EmployeeCharge[] Charges { get; set; }
    public EmployeeBonus[] Bonuses { get; set; }
    public EmployeeDeduction[] Deductions { get; set; }
    public EmployeeLoan[] Loans { get; set; }

    public EmployeeCalculateEarningData() : base() {

    }
    public EmployeeCalculateEarningData(Employee emp) : base(emp) {
    }
  }
}