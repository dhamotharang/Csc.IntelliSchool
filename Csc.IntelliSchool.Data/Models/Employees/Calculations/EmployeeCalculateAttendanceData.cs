using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  /// <summary>
  /// Filtered employee data used for attendance and earning calculations.
  /// </summary>
  public class EmployeeCalculateAttendanceData : EmployeeCalculateData{
    public EmployeeShift Shift { get; set; }

    public HashSet<EmployeeAttendance> Attendance { get; set; }
    public EmployeeTerminalTransaction[] Transactions { get; set; }
    public DateTime[] PaidVacationDays { get; set; }
    public DateTime[] UnpaidVacationDays { get; set; }

    public EmployeeCalculateAttendanceData() : base() {

    }
    public EmployeeCalculateAttendanceData(Employee emp) : base(emp) {
    }
  }
}