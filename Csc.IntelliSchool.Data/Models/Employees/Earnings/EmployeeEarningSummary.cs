
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  public class EmployeeEarningSummary : IEmployeeEarning {
    public Employee Employee { get; set; }
    public EmployeeDepartment Department { get; set; }

    public string Key {
      get {
        if (Employee != null && Employee.Person != null)
          return Employee.Person.FullName;
        else if (Department != null)
          return Department.Name;
        return null;
      }
    }
    public int Count { get; set; }

    // Salary
    public int Salary { get; set; }
    public int Social { get; set; }
    public int Medical { get; set; }
    public int Taxes { get; set; }
    public int Allowances { get; set; }
    public int Charges { get; set; }
    [IgnoreDataMember]
    [NotMapped]
    public int Gross { get { return Salary - Social - Medical - Taxes + Allowances - Charges; } }

    // Bonuses
    public decimal BonusPoints { get; set; }
    public int BonusValues { get; set; }
    public int BonusTotalValue { get; set; }

    // Deductions
    public decimal DeductionPoints { get; set; }
    public int DeductionValues { get; set; }
    public int DeductionTotalValue { get; set; }

    // Lonans
    public int Loans { get; set; }

    // Absences
    public int AbsenceDays { get; set; }
    public int AbsenceExtraDays { get; set; }
    public int UnpaidVacationDays { get; set; }
    public int PaidVacationDays { get; set; }
    public int UnemploymentDays { get; set; }
    public int AbsenceTotalValue { get; set; }

    // Attendance
    public decimal AttendancePoints { get; set; }
    public decimal TimeOffPoints { get; set; }
    public decimal OvertimePoints { get; set; }

    public int AttendanceValue { get; set; }
    public int TimeOffValue { get; set; }
    public int OvertimeValue { get; set; }
    public int AttendanceTotalValue { get { return AttendanceValue + TimeOffValue - OvertimeValue; }  }

    // Adjustment
    public int Adjustment { get; set; }

    [IgnoreDataMember]
    [NotMapped]
    public int Net { get { return Gross + BonusTotalValue - DeductionTotalValue - Loans - AbsenceTotalValue - AttendanceTotalValue + Adjustment; } }

    public static EmployeeEarningSummary Create(EmployeeDepartment dept, IEnumerable<IEmployeeEarning> earnings) {
      var sum = Create(earnings);
      sum.Department = dept;
      return sum;
    }

    public static EmployeeEarningSummary Create(Employee emp, IEnumerable<IEmployeeEarning> earnings) {
      var sum = Create(earnings);
      sum.Employee = emp;
      return sum;
    }

    public static EmployeeEarningSummary Create( IEnumerable<IEmployeeEarning> earnings) {
      EmployeeEarningSummary sum = new EmployeeEarningSummary();
      sum.Count = earnings.Count();

      ///////////////////////////
      // Salary
      sum.Salary = earnings.Sum(s => s.Salary);
      sum.Social = earnings.Sum(s => s.Social);
      sum.Medical = earnings.Sum(s => s.Medical);
      sum.Taxes = earnings.Sum(s => s.Taxes);
      sum.Allowances = earnings.Sum(s => s.Allowances);
      sum.Charges = earnings.Sum(s => s.Charges);

      ///////////////////////////
      // Bonuses
      sum.BonusPoints = earnings.Sum(s => s.BonusPoints);
      sum.BonusValues = earnings.Sum(s => s.BonusValues);
      sum.BonusTotalValue = earnings.Sum(s => s.BonusTotalValue);

      ///////////////////////////
      // Deductionsw
      sum.DeductionPoints = earnings.Sum(s => s.DeductionPoints);
      sum.DeductionValues = earnings.Sum(s => s.DeductionValues);
      sum.DeductionTotalValue = earnings.Sum(s => s.DeductionTotalValue);

      ///////////////////////////
      // Loans
      sum.Loans = earnings.Sum(s => s.Loans);

      ///////////////////////////
      // Absences
      sum.AbsenceDays = earnings.Sum(s => s.AbsenceDays);
      sum.AbsenceExtraDays = earnings.Sum(s => s.AbsenceExtraDays);
      sum.UnpaidVacationDays = earnings.Sum(s => s.UnpaidVacationDays);
      sum.PaidVacationDays = earnings.Sum(s => s.PaidVacationDays);
      sum.UnemploymentDays = earnings.Sum(s => s.UnemploymentDays);
      sum.AbsenceTotalValue = earnings.Sum(s => s.AbsenceTotalValue);

      ///////////////////////////
      // Attendance
      sum.AttendancePoints = earnings.Sum(s => s.AttendancePoints);
      sum.AttendanceValue = earnings.Sum(s => s.AttendanceValue);
      sum.TimeOffPoints = earnings.Sum(s => s.TimeOffPoints);
      sum.TimeOffValue = earnings.Sum(s => s.TimeOffValue);
      sum.OvertimePoints = earnings.Sum(s => s.OvertimePoints);
      sum.OvertimeValue = earnings.Sum(s => s.OvertimeValue);

      ///////////////////////////
      // Other
      sum.Adjustment = earnings.Sum(s => s.Adjustment);

      return sum;
    }

  }


}


