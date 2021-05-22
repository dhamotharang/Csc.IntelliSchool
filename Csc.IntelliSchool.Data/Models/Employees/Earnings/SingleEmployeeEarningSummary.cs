
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public class SingleEmployeeEarningSummary {
    public int EmployeeID { get; set; }
    public int EarningID { get; set; }
    public DateTime Month { get; set; }

    public int Salary { get; set; }
    public int Gross { get; set; }
    public int Bonuses { get; set; }

    public int Deductions { get; set; }
    public int Attendance { get; set; }
    public int Loans { get; set; }
    public int Adjustment { get; set; }

    public int Net { get { return Gross + Bonuses - Deductions - Attendance - Loans + Adjustment; } }

    public static SingleEmployeeEarningSummary Create(EmployeeEarning s) {
      SingleEmployeeEarningSummary sum = new Data.SingleEmployeeEarningSummary();

      sum.EmployeeID = s.EmployeeID;
      sum.EarningID = s.EarningID;
      sum.Month = s.Month;
      sum.Salary = s.Salary;
      sum.Gross = s.Gross;
      sum.Bonuses = s.BonusTotalValue;
      sum.Deductions = s.DeductionTotalValue;
      sum.Attendance = s.AttendanceTotalValue;
      sum.Loans = s.Loans;
      sum.Adjustment = s.Adjustment;

      return sum;
    }
  }


}