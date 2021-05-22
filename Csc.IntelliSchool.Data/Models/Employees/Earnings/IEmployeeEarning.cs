namespace Csc.IntelliSchool.Data {
  public interface IEmployeeEarning : IEmployeeRelation {
    // Salary
    int Salary { get; }
    int Social { get; }
    int Medical { get; }
    int Taxes { get; }
    int Allowances { get; }
    int Charges { get; }
    int Gross { get; }

    // Bonuses
    decimal BonusPoints { get; }
    int BonusValues { get; }
    int BonusTotalValue { get; }

    // Deductions
    decimal DeductionPoints { get; }
    int DeductionValues { get; }
    int DeductionTotalValue { get; }

    // Loans
    int Loans { get; }

    // Absences
    int AbsenceDays { get; }
    int AbsenceExtraDays { get; }
    int UnpaidVacationDays { get; }
    int PaidVacationDays { get; }
    int UnemploymentDays { get; }
    int AbsenceTotalValue { get; }

    // Attendance
    decimal AttendancePoints { get; }
    int AttendanceValue { get; }
    decimal TimeOffPoints { get; }
    int TimeOffValue{ get; }
    decimal OvertimePoints { get; }
    int OvertimeValue { get; }
    int AttendanceTotalValue { get; }

    // Adjustment
    int Adjustment { get; }
  }
}