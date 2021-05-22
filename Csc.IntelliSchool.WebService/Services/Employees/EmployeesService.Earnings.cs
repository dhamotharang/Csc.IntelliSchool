using System;
using System.ServiceModel;
using System.IO;
using System.Linq;
using Csc.Components.Common;
using System.Collections.Generic;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    public void RecalculateEmployeeEarnings(DateTime month, int[] employeeIds, EmployeeRecalculateFlags optionFlags) {
      optionFlags |= EmployeeRecalculateFlags.Earning;

      month = month.ToMonth();
      DateTime startDate = month;
      DateTime endDate = startDate.ToMonthEnd();

      using (var ent = ServiceManager.CreateModel(false)) {
        List<EmployeeEarning> earningList = ent.EmployeeEarnings
          .Include(DataModelExtensions.GetEmployeeIncludes(EmployeeDataFilter.Employment | EmployeeDataFilter.ShiftOverridesTerminal | EmployeeDataFilter.Salary, true))
          .Where(s => employeeIds.Contains(s.EmployeeID) && s.Month == month).ToList();
        InternalDeleteDuplicateEmployeeEarnings(ent, earningList);


        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(optionFlags);
        options.Month = month;
        options.Model = ent;
        options.Flags = InternalGetHumanResourcesFlags(ent);

        if (employeeIds.Count() > 0) {
          var attendance = InternalGetEmployeeAttendance(options, employeeIds);

          // load recaculated employees
          var employees = ent.Employees
            .Query(EmployeeDataFilter.Employment | EmployeeDataFilter.ShiftOverridesTerminal | EmployeeDataFilter.Salary, null, null, employeeIds).ToArray();

          var deductions = ent.EmployeeDeductions.Where(s => employeeIds.Contains(s.EmployeeID) && s.Date >= startDate && s.Date <= endDate).ToArray();
          var loans = ent.EmployeeLoanInstallments.Include("Loan").Where(s => s.Month == month).ToArray().Where(s => employeeIds.Contains(s.Loan.EmployeeID)).ToArray();
          var bonuses = ent.EmployeeBonuses.Where(s => employeeIds.Contains(s.EmployeeID) && s.Date >= startDate && s.Date <= endDate).ToArray();

          foreach (var emp in employees) {
            InternalCalculateSingleEmployeeEarning(options,
              emp, earningList,
              attendance, deductions, loans, bonuses);
          }


          options.Model.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Calculate, typeof(EmployeeEarning), null,
            new SystemLogDataEntry() { EmployeeIDs = employees.Select(s => s.EmployeeID).ToArray(), EmployeeRecalculateFlags = options.OptionFlags, Month = options.Month });

          ent.SaveChanges();
        }
      }
    }

    public EmployeeEarning[] GetEmployeeEarnings(DateTime month, int[] listIds, int[] employeeIds, EmploeeEarningCalculationMode mode) {
      month = month.ToMonth();
      DateTime startDate = month;
      DateTime endDate = startDate.ToMonthEnd();

      using (var ent = ServiceManager.CreateModel(false)) {
        employeeIds = ent.Employees.Query(EmployeeDataFilter.None, month, listIds, employeeIds).Select(s => s.EmployeeID).ToArray();

        List<EmployeeEarning> earningList = ent.EmployeeEarnings
          .Include(DataModelExtensions.GetEmployeeIncludes(EmployeeDataFilter.Personal | EmployeeDataFilter.Employment | EmployeeDataFilter.ShiftOverridesTerminal | EmployeeDataFilter.Salary, true))
          .Where(s => employeeIds.Contains(s.EmployeeID) && s.Month == month).ToList();
        InternalDeleteDuplicateEmployeeEarnings(ent, earningList);


        int[] tmpRecalcEmployeeIds = new int[] { };
        if (mode != EmploeeEarningCalculationMode.None) // recalc all
          tmpRecalcEmployeeIds = employeeIds.ToArray();
        // append not found
        employeeIds = tmpRecalcEmployeeIds.Concat(employeeIds.Except(earningList.Select(s => s.EmployeeID).ToArray())).Distinct().ToArray();


        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(mode != EmploeeEarningCalculationMode.None);
        options.Month = month;
        options.Model = ent;
        options.Flags = InternalGetHumanResourcesFlags(ent);

        if (mode != EmploeeEarningCalculationMode.None)
          options.OptionFlags |= EmployeeRecalculateFlags.EditedEarning;
        if (mode == EmploeeEarningCalculationMode.Basic)
          options.OptionFlags |= EmployeeRecalculateFlags.BasicEarning;

        if (employeeIds.Count() > 0) {
          var attendance = InternalGetEmployeeAttendance(options, employeeIds);

          // load recaculated employees
          var employees = ent.Employees
            .Query(EmployeeDataFilter.Employment | EmployeeDataFilter.ShiftOverridesTerminal | EmployeeDataFilter.Salary, null, null, employeeIds).ToArray();

          var deductions = ent.EmployeeDeductions.Where(s => employeeIds.Contains(s.EmployeeID) && s.Date >= startDate && s.Date <= endDate).ToArray();
          var loans = ent.EmployeeLoanInstallments.Include("Loan").Where(s => s.Month == month).ToArray().Where(s => employeeIds.Contains(s.Loan.EmployeeID)).ToArray();
          var bonuses = ent.EmployeeBonuses.Where(s => employeeIds.Contains(s.EmployeeID) && s.Date >= startDate && s.Date <= endDate).ToArray();

          foreach (var emp in employees) {
            InternalCalculateSingleEmployeeEarning(options,
              emp, earningList,
              attendance, deductions, loans, bonuses);
          }

          options.Model.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Calculate, typeof(EmployeeEarning), null,
            new SystemLogDataEntry() { EmployeeIDs = employees.Select(s => s.EmployeeID).ToArray(), EmployeeRecalculateFlags = options.OptionFlags, Month = options.Month });

          ent.SaveChanges();
        }


        return earningList.ToArray();
      }
    }

    private void InternalDeleteDuplicateEmployeeEarnings(DataEntities ent, List<EmployeeEarning> earningList) {
      foreach (var grp in earningList.OrderByDescending(s => s.EarningID).GroupBy(s => s.EmployeeID).Where(s => s.Count() > 1).ToArray()) {
        foreach (var itm in grp.Skip(1).ToArray()) {
          ent.EmployeeEarnings.Remove(itm);
          earningList.Remove(itm);
        }
      }
    }

    private static EmployeeEarning InternalCalculateSingleEmployeeEarning(EmployeeRecalculateOptionFlags options,
      Employee emp,
      List<EmployeeEarning> allEarnings,
      EmployeeAttendance[] allAttendance,
      EmployeeDeduction[] allDeductions,
      EmployeeLoanInstallment[] allLoans,
      EmployeeBonus[] allBonuses) {

      EmployeeEarning earning = allEarnings.SingleOrDefault(s => s.EmployeeID == emp.EmployeeID);
      if (earning == null) {
        earning = new EmployeeEarning() { EmployeeID = emp.EmployeeID, Employee = emp, Month = options.Month };
        allEarnings.Add(earning);
      } else if (options.CalculateEditedEarning == false && earning.IsEdited) // skip
        return earning;
      else
        options.Model.SetModified(earning);

      earning.IsEdited = false;
      earning.Notes = null;

      DateTime startDate = earning.Month.ToMonth();
      DateTime endDate = earning.Month.ToMonthEnd();

      //////////////////////////////////////////////////////////////////////////
      // batch loading if null
      if (options.Flags == null)
        options.Flags = InternalGetHumanResourcesFlags(options.Model);
      if (allAttendance == null)
        allAttendance = options.Model.EmployeeAttendance.Include("TimeOffs").Where(s => s.EmployeeID == emp.EmployeeID && s.Date >= startDate && s.Date <= endDate).OrderBy(s => s.Date).ToArray();
      if (allDeductions == null)
        allDeductions = options.Model.EmployeeDeductions.Where(s => s.EmployeeID == emp.EmployeeID && s.Date >= startDate && s.Date <= endDate).OrderBy(s => s.Date).ToArray();
      if (allLoans == null)
        allLoans = options.Model.EmployeeLoanInstallments.Include("Loan").Where(s => s.Loan.EmployeeID == emp.EmployeeID && s.Month == earning.Month).ToArray();
      if (allBonuses == null)
        allBonuses = options.Model.EmployeeBonuses.Where(s => s.EmployeeID == emp.EmployeeID && s.Date >= startDate && s.Date <= endDate).OrderBy(s => s.Date).ToArray();

      //////////////////////////////////////////////////////////////////////////
      // load employee items
      var attendance = allAttendance.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      var deductions = allDeductions.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      var loans = allLoans.Where(s => s.Loan.EmployeeID == emp.EmployeeID).ToArray();
      var bonuses = allBonuses.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();

      //////////////////////////////////////////////////////////////////////////
      // salary data
      earning.Salary = emp.Salary.Salary;
      earning.Housing = emp.Salary.Housing;
      earning.Travel = emp.Salary.Travel;
      earning.Social = emp.Salary.Social;
      earning.Medical = emp.Salary.Medical;
      earning.Taxes = emp.Salary.Taxes;
      earning.Allowance = emp.Salary.Allowance;
      earning.Expenses = emp.Salary.Expenses;

      //////////////////////////////////////////////////////////////////////////
      // deductions
      earning.DeductionPoints = deductions.Where(s => s.Points != null).Sum(s => s.Points) ?? 0;
      earning.DeductionValues = deductions.Where(s => s.Value != null).Sum(s => s.Value) ?? 0;

      //////////////////////////////////////////////////////////////////////////
      // Loans
      earning.Loans = loans.Sum(s => s.Amount);

      //////////////////////////////////////////////////////////////////////////
      // Bonuses
      earning.BonusPoints = bonuses.Where(s => s.Points != null).Sum(s => s.Points) ?? 0;
      earning.BonusValues = bonuses.Where(s => s.Value != null).Sum(s => s.Value) ?? 0;

      ////////////////////////////////////////////////////////////////////////////
      // Attendance
      if (earning.EarningID == 0 || options.CalculateBasicEarning == false) {
        //////////////////////////////////////////////////////////////////////////
        // unemployment
        earning.UnemploymentDays = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Unemployed).Count()
          + attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Upcoming).Count();
        if (earning.UnemploymentDays > 30)
          earning.UnemploymentDays = 30;

        //////////////////////////////////////////////////////////////////////////
        // absences
        earning.AbsenceDays = attendance.Sum(s => s.AbsencePoints) ?? 0;
        if (options.Flags.CalculateExtraAbsences == true)
          earning.AbsenceExtraDays = attendance.Sum(s => s.ExtraAbsencePoints) ?? 0;
        else
          earning.AbsenceExtraDays = 0;

        //////////////////////////////////////////////////////////////////////////
        // attendance
        earning.AttendancePoints = attendance.Sum(s => s.AttendancePoints) ?? 0;

        //////////////////////////////////////////////////////////////////////////
        // Time-Offs
        earning.TimeOffPoints = attendance.Sum(s => s.TimeOffPoints) ?? 0;
      }

      //////////////////////////////////////////////////////////////////////////
      // Net
      //earning.Net = earning.CalculatedNet;

      if (earning.EarningID == 0)
        options.Model.EmployeeEarnings.Add(earning);

      return earning;
    }

    public EmployeeEarning UpdateEmployeeEarning(EmployeeEarning earning) {
      //earning.Net = earning.CalculatedNet;

      using (var ent = ServiceManager.CreateModel()) {
        var dbEarning = ent.EmployeeEarnings.SingleOrDefault(s => s.EmployeeID == earning.EmployeeID && s.Month == earning.Month);

        if (dbEarning == null) {
          earning.EarningID = 0;
          ent.EmployeeEarnings.Add(earning);
        } else {
          ent.Entry(dbEarning).CurrentValues.SetValues(earning);
        }



        dbEarning.IsEdited = true;
        ent.SaveChanges();

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbEarning != null ? SystemLogDataAction.Update : SystemLogDataAction.Insert, typeof(EmployeeEarning), null,
          new SystemLogDataEntry () { EmployeeID = earning.EmployeeID, Month = earning.Month });
        ent.Logger.Flush();

        return dbEarning == null ? earning : dbEarning;
      }
    }

    public SingleEmployeeEarningSummary[] GetSingleEmployeeEarningsSummary(int employeeId, int year, PeriodFilter filter) {
      using (var ent = ServiceManager.CreateModel()) {
        var qry = ent.EmployeeEarnings.Where(s => s.EmployeeID == employeeId);

        switch (filter) {
          case PeriodFilter.Current:
            qry = qry.Where(s => s.Month.Year == year);
            break;
          case PeriodFilter.Upcoming:
            qry = qry.Where(s => s.Month.Year > year);
            break;
          case PeriodFilter.Past:
            qry = qry.Where(s => s.Month.Year < year);
            break;
          default:
            break;
        }

        return qry.ToArray().Select(s => new SingleEmployeeEarningSummary() {
          EmployeeID = employeeId,
          EarningID = s.EarningID,
          Month = s.Month,
          Salary = s.Salary,
          Gross = s.Gross,
          Bonuses = s.BonusTotalValue,
          Deductions = s.DeductionTotalValue,
          Attendance = s.AttendanceTotal,
          Loans = s.Loans
        }).ToArray();

      }
    }

    public EmployeeEarningSummary[] GetEmployeeEarningsSummary(DateTime startMonth, DateTime endMonth, int[] listIds) {
      startMonth = startMonth.ToMonth();
      endMonth = endMonth.ToMonth();

      DateTime endDate = endMonth.ToMonthEnd();

      using (var ent = ServiceManager.CreateModel()) {
        var employees = ent.Employees.Query(EmployeeDataFilter.Default, null, listIds, null)
          .Where(s => (s.TerminationDate == null || startMonth <= s.TerminationDate) && s.HireDate <= endDate).ToArray();
        var employeeIds = employees.Select(s => s.EmployeeID).ToArray();
        var allEarnings = ent.EmployeeEarnings.Where(s => employeeIds.Contains(s.EmployeeID) && s.Month >= startMonth && s.Month <= endMonth).ToArray();

        List<EmployeeEarningSummary> results = new List<EmployeeEarningSummary>(employees.Count());
        foreach (var emp in employees) {
          var earnings = allEarnings.Where(s => s.EmployeeID == emp.EmployeeID && emp.IsMonthEmployee(s.Month)).ToArray();
          if (earnings.Count() == 0)
            continue;

          EmployeeEarningSummary sum = new EmployeeEarningSummary();
          sum.Employee = emp;
          sum.MonthCount = earnings.Count();

          sum.Salary = earnings.Sum(s => s.Salary);
          sum.Housing = earnings.Sum(s => s.Housing);
          sum.Travel = earnings.Sum(s => s.Travel);
          sum.Social = earnings.Sum(s => s.Social);
          sum.Medical = earnings.Sum(s => s.Medical);
          sum.Taxes = earnings.Sum(s => s.Taxes);
          sum.Allowance = earnings.Sum(s => s.Allowance);
          sum.Expenses = earnings.Sum(s => s.Expenses);

          sum.BonusTotalValue = earnings.Sum(s => s.BonusTotalValue);
          sum.DeductionTotalValue = earnings.Sum(s => s.DeductionTotalValue);

          sum.UnemploymentDays = earnings.Sum(s => s.UnemploymentDays);
          sum.UnemploymentValue = earnings.Sum(s => s.UnemploymentValue);

          sum.Loans = earnings.Sum(s => s.Loans);

          sum.AbsenceDays = earnings.Sum(s => s.AbsenceDays);
          sum.AbsenceDaysValue = earnings.Sum(s => s.AbsenceDaysValue);
          sum.AbsenceExtraDays = earnings.Sum(s => s.AbsenceExtraDays);
          sum.AbsenceExtraDaysValue = earnings.Sum(s => s.AbsenceExtraDaysValue);

          sum.AttendancePoints = earnings.Sum(s => s.AttendancePoints);
          sum.AttendanceValue = earnings.Sum(s => s.AttendanceValue);
          sum.TimeOffPoints = earnings.Sum(s => s.TimeOffPoints);
          sum.TimeOffValue = earnings.Sum(s => s.TimeOffValue);

          results.Add(sum);
        }

        return results.ToArray();
      }

    }
  }

}
