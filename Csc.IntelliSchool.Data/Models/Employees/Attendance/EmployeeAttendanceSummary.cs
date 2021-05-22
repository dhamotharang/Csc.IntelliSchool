using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  public class EmployeeAttendanceSummary  {
    public Employee Employee { get; set; }

    public string Key {
      get {
        if (Employee != null && Employee.Person != null)
          return Employee.Person.FullName;
        return null;
      }
    }

    // Absences
    public int UnemploymentDays { get; set; }
    public int AbsenceDays { get; set; }
    public int AbsenceExtraDays { get; set; }
    public int UnpaidVacationDays { get; set; }
    public int PaidVacationDays { get; set; }

    // Attendance
    public int? AttendancePointsDays { get; set; }
    public decimal AttendancePoints { get; set; }
    public decimal TimeOffPoints { get; set; }
    public decimal OvertimePoints { get; set; }

    public static EmployeeAttendanceSummary Create(Employee emp, IEnumerable<IEmployeeEarning> earnings) {
      EmployeeAttendanceSummary sum = new EmployeeAttendanceSummary();
      sum.Employee = emp;

      // Absences
      sum.AbsenceDays = earnings.Sum(s => s.AbsenceDays);
      sum.AbsenceExtraDays = earnings.Sum(s => s.AbsenceExtraDays);
      sum.UnpaidVacationDays = earnings.Sum(s => s.UnpaidVacationDays);
      sum.PaidVacationDays = earnings.Sum(s => s.PaidVacationDays);
      sum.UnemploymentDays = earnings.Sum(s => s.UnemploymentDays);

      // Attendance
      sum.AttendancePointsDays = null;
      sum.AttendancePoints= earnings.Sum(s => s.AttendancePoints);
      sum.TimeOffPoints = earnings.Sum(s => s.TimeOffPoints);
      sum.OvertimePoints = earnings.Sum(s => s.OvertimePoints);

      return sum;
    }

    public static EmployeeAttendanceSummary Create(Employee emp, IEnumerable<EmployeeAttendance> attendance) {
      EmployeeAttendanceSummary sum = new EmployeeAttendanceSummary();
      sum.Employee = emp;

      // Absences
      sum.AbsenceDays = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Absent).Sum(s => s.AbsencePoints ?? 0);
      sum.AbsenceExtraDays = attendance.Sum(s => s.ExtraAbsencePoints ?? 0);
      sum.UnpaidVacationDays = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.UnpaidVacation).Sum(s => s.AbsencePoints ?? 0);
      sum.PaidVacationDays = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.PaidVacation).Count();
      sum.UnemploymentDays = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Unemployed || s.AttendanceStatus == EmployeeAttendanceStatus.Upcoming).Sum(s => s.AbsencePoints ?? 0);

      // Attendance
      sum.AttendancePointsDays = attendance.Where(s => s.TotalAttendancePoints != 0).Count();
      sum.AttendancePoints = attendance.Sum(s => s.TotalAttendancePoints ?? 0);
      sum.TimeOffPoints = attendance.Sum(s => s.TotalTimeOffPoints ?? 0);
      sum.OvertimePoints =  attendance.Sum(s => s.TotalOvertimePoints ?? 0);

      return sum;
    }
  }
}