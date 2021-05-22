using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    private static EmployeeEarning InternalCalculateSingleEmployeeEarning(
      Employee emp,
      EmployeeCalculateOptions options,
      EmployeeCalculateEarningState operationState) {

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 1. ensure that required data is loaded
      InternalLoadCalculateEarningState(emp.PackArray(), options, operationState);

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 2. load specific employee items
      var employeeData = InternalCreateEmployeeCalculateEarningData(emp, options, operationState);

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 3. if null create one
      if (employeeData.Earning == null) {
        employeeData.Earning = new EmployeeEarning() {
          EmployeeID = emp.EmployeeID,
          Employee = emp,
          Month = options.Month
        };
        operationState.Earnings.Add(employeeData.Earning);
        operationState.Model.EmployeeEarnings.Add(employeeData.Earning);
      } else if (options.CalculateEditedEarning == false && employeeData.Earning.IsEdited)// skip
        return employeeData.Earning;
      else {
        operationState.Model.SetModified(employeeData.Earning);
      }

      employeeData.Earning.Notes = null;
      employeeData.Earning.IsEdited = false;

      DateTime startDate = employeeData.Earning.Month.ToMonth();
      DateTime endDate = employeeData.Earning.Month.ToMonthEnd();

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 4. calculate

      /////////////////////////////////
      // a. salary
      if (employeeData.Salary != null) {
        employeeData.Earning.Salary =  employeeData.Salary.Salary;
        employeeData.Earning.Social = employeeData.Salary.Social;
        employeeData.Earning.Medical = employeeData.Salary.Medical;
        employeeData.Earning.Taxes = employeeData.Salary.Taxes;
      } else {
        employeeData.Earning.Salary =
          employeeData.Earning.Social =
          employeeData.Earning.Medical =
          employeeData.Earning.Taxes = 0;
      }

      /////////////////////////////////
      // b. allowances / charges
      employeeData.Earning.Allowances = employeeData.Allowances.Sum(s => s.Value);
      employeeData.Earning.Charges = employeeData.Charges.Sum(s => s.Value);


      /////////////////////////////////
      // c. Bonuses
      employeeData.Earning.BonusPoints = employeeData.Bonuses.Sum(s => s.Points ?? 0);
      employeeData.Earning.BonusValues = employeeData.Bonuses.Sum(s => s.Value ?? 0);

      /////////////////////////////////
      // d. deductions
      employeeData.Earning.DeductionPoints = employeeData.Deductions.Sum(s => s.Points ?? 0);
      employeeData.Earning.DeductionValues = employeeData.Deductions.Sum(s => s.Value ?? 0);

      /////////////////////////////////
      // e. Loans
      employeeData.Earning.Loans = employeeData.Loans.SelectMany(s => s.Installments).Sum(s => s.Amount);

      /////////////////////////////////
      // Attendance
      if (employeeData.Earning.EarningID == 0 || options.CalculateEarningSalariesOnly == false) {
        /////////////////////////////////
        // f. absence days
        employeeData.Earning.AbsenceDays =
          employeeData.Attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Absent).Sum(s => s.AbsencePoints ?? 0);

        /////////////////////////////////
        // g. absence extra days
        employeeData.Earning.AbsenceExtraDays = employeeData.Attendance.Sum(s => s.ExtraAbsencePoints ?? 0);

        /////////////////////////////////
        // h. unpaid vacations
        employeeData.Earning.UnpaidVacationDays =
          employeeData.Attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.UnpaidVacation).Sum(s => s.AbsencePoints ?? 0);
        employeeData.Earning.PaidVacationDays =
          employeeData.Attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.PaidVacation).Count();

        /////////////////////////////////
        // i. unemployment
        employeeData.Earning.UnemploymentDays =
          employeeData.Attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Unemployed || s.AttendanceStatus == EmployeeAttendanceStatus.Upcoming)
          .Sum(s => s.AbsencePoints ?? 0);

        /////////////////////////////////
        // j. attendance points
        employeeData.Earning.AttendancePoints = employeeData.Attendance.Sum(s => s.TotalAttendancePoints ?? 0);

        /////////////////////////////////
        // k. attendance timeoffs
        employeeData.Earning.TimeOffPoints = employeeData.Attendance.Sum(s => s.TotalTimeOffPoints ?? 0);

        /////////////////////////////////
        // l. attendance overtime
        employeeData.Earning.OvertimePoints = employeeData.Attendance.Sum(s => s.TotalOvertimePoints ?? 0);
      }

      /////////////////////////////////
      // Net
      employeeData.Earning.RecalculateAdjustment();

      return employeeData.Earning;
    }

  }
}