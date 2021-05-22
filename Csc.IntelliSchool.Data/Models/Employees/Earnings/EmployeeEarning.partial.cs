
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeEarning : IEmployeeEarning {
    [IgnoreDataMember]
    [NotMapped]
    public int DailySalary { get { return (int)Math.Round((decimal)Salary / 30L); } }
    [IgnoreDataMember]
    [NotMapped]
    public int Gross { get { return Salary - Social - Medical - Taxes + Allowances - Charges; } }

    #region Bonuses
    [IgnoreDataMember]
    [NotMapped]
    public int BonusPointsValue { get { return (int)Math.Round(BonusPoints * (decimal)DailySalary); } }
    [IgnoreDataMember]
    [NotMapped]
    public int BonusTotalValue { get { return BonusPointsValue + BonusValues; } }
    #endregion

    #region Deductions
    [IgnoreDataMember]
    [NotMapped]
    public int DeductionPointsValue { get { return (int)Math.Round(DeductionPoints * (decimal)DailySalary); } }
    [IgnoreDataMember]
    [NotMapped]
    public int DeductionTotalValue { get { return DeductionPointsValue + DeductionValues; } }
    #endregion

    #region Absences
    [IgnoreDataMember]
    [NotMapped]
    public int AbsenceDaysValue { get { return AbsenceDays * DailySalary; } }
    [IgnoreDataMember]
    [NotMapped]
    public int AbsenceExtraDaysValue { get { return AbsenceExtraDays * DailySalary; } }
    [IgnoreDataMember]
    [NotMapped]
    public int UnpaidVacationsValue { get { return UnpaidVacationDays * DailySalary; } }
    [IgnoreDataMember]
    [NotMapped]
    public int UnemploymentValue { get { return UnemploymentDays * DailySalary; } }
    [IgnoreDataMember]
    [NotMapped]
    public int AbsenceTotalValue { get { return AbsenceDaysValue + AbsenceExtraDaysValue + UnpaidVacationsValue + UnemploymentValue; } }

    #endregion

    #region Attendance
    [IgnoreDataMember]
    [NotMapped]
    public int AttendanceValue { get { return (int)Math.Round(AttendancePoints * (decimal)DailySalary); } }
    [IgnoreDataMember]
    [NotMapped]
    public int TimeOffValue { get { return (int)Math.Round(TimeOffPoints * (decimal)DailySalary); } }
    [IgnoreDataMember]
    [NotMapped]
    public int OvertimeValue { get { return (int)Math.Round(OvertimePoints * (decimal)DailySalary); } }
    [IgnoreDataMember]
    [NotMapped]
    public int AttendanceTotalValue { get { return AttendanceValue + TimeOffValue - OvertimeValue; } }
    #endregion

    [IgnoreDataMember]
    [NotMapped]
    public DateTime? LastUpdated { get { return LastUpdatedUtc != null ? LastUpdatedUtc.Value.ToLocalTime() : new DateTime?(); } }


    [IgnoreDataMember]
    [NotMapped]
    public int NetBeforeAdjustment {
      get {
        return Gross
          + BonusTotalValue
          - DeductionTotalValue
          - Loans
          - AbsenceTotalValue
          - AttendanceTotalValue;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int Net {
      get {
        return NetBeforeAdjustment + Adjustment;
      }
    }

    public void RecalculateAdjustment() {
      if (NetBeforeAdjustment < 0)
        Adjustment = Math.Abs(NetBeforeAdjustment);
    }
  }
}