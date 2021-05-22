
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeEarnings", Schema = "HR")]
  public partial class EmployeeEarning {
    [Key]
    public int EarningID { get; set; }

    public System.DateTime Month { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public Nullable<System.DateTime> LastUpdatedUtc { get; set; }


    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }

    #region Salary
    public int Salary { get; set; }
    public int Social { get; set; }
    public int Medical { get; set; }
    public int Taxes { get; set; }
    #endregion

    #region Salary Changes
    public int Allowances { get; set; }
    public int Charges { get; set; }
    #endregion

    #region Bonuses
    public decimal BonusPoints { get; set; }
    public int BonusValues { get; set; }
    #endregion

    #region Deductions
    public decimal DeductionPoints { get; set; }
    public int DeductionValues { get; set; }
    #endregion

    #region Loans
    public int Loans { get; set; }
    #endregion

    #region Absences
    public int AbsenceDays { get; set; }
    public int AbsenceExtraDays { get; set; }
    public int UnemploymentDays { get; set; }
    public int UnpaidVacationDays { get; set; }
    public int PaidVacationDays { get; set; }


    #endregion

    #region Attendance
    public decimal AttendancePoints { get; set; }
    public decimal TimeOffPoints { get; set; }
    public decimal OvertimePoints { get; set; }
    #endregion

    #region Adjustment
    public int Adjustment { get; set; }
    #endregion


    public bool IsEdited { get; set; }
    public string Notes { get; set; }

  }
}
