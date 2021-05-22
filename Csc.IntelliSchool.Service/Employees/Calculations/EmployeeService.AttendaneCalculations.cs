using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    private static IEnumerable<EmployeeAttendance> InternalCalculateSingleEmployeeAttendance(
      Employee emp,
      EmployeeCalculateOptions options,
      EmployeeCalculateAttendanceState operationState) {

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 1. ensure that required data is loaded
      InternalLoadCalculateAttendanceState(emp.PackArray(), options, operationState);

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 2. load specific employee items
      var employeeData = InternalCreateEmployeeCalculateAttendanceData(emp, options, operationState);

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 3. Calculate 
      DateTime date = options.StartDate.ToDay();
      while (date <= options.EndDate.ToDay()) {
        var att = InternalGetCalculateAttendanceItem(date, options, operationState, employeeData);

        if (att == null)
          continue;

        /////////////////////////////////////////////////////////////////////////////////////////////////
        // 3.c. set attendance data
        InternalSetAttendanceData(att, options, operationState, employeeData);

        /////////////////////////////////////////////////////////////////////////////////////////////////
        // 3.d. move forward
        date = date.AddDays(1);
      }

      if (employeeData.Shift != null && employeeData.Shift.CalculateExtraAbsences) {
        employeeData.Attendance = employeeData.Attendance.OrderBy(s => s.Date).ToHashSet();
        InternalSetEmployeeExtraAttendancePoints(employeeData.Attendance);
      }

      return employeeData.Attendance;
    }

    internal static EmployeeAttendance InternalGetCalculateAttendanceItem(
      DateTime date,
      EmployeeCalculateOptions options,
      EmployeeCalculateAttendanceState operationState,
      EmployeeCalculateAttendanceData employeeData) {

      //////////////////
      // 3.a. Get date attendance
      var att = employeeData.Attendance.SingleOrDefault(s => s.Date == date);

      //////////////////
      // 3.b. if not available, create one
      if (att == null) {
        att = new EmployeeAttendance() {
          Date = date,
          EmployeeID = employeeData.EmployeeID,
        };

        employeeData.Attendance.Add(att);
        operationState.Attendance.Add(att);
        operationState.Model.EmployeeAttendance.Add(att);
      } else if // attendance not null
        ((options.CalculateAttendance == false) || //available and recalculate == false
        (options.CalculatedEditedAttendance == false && att.IsEdited) ||  // exclude edited, item is edited
        (options.CalculatedLockedAttendance == false && att.IsLocked))  // exclude locked, item is locked
        return null;
      else
        operationState.Model.SetModified(att);

      ////////////////////////////////////////////
      // recalculate or a new item
      att.ClearData();
      foreach (var timeOff in att.TimeOffs.ToArray()) {
        operationState.Model.EmployeeAttendanceTimeOffs.Remove(timeOff);
      }

      return att;
    }


    internal static void InternalSetAttendanceData(
      EmployeeAttendance att,
      EmployeeCalculateOptions options,
      EmployeeCalculateAttendanceState operationState,
      EmployeeCalculateAttendanceData employeeData) {

      if (att.Date < employeeData.Employee.HireDate || (employeeData.Employee.TerminationDate != null && employeeData.Employee.TerminationDate < att.Date)) {
        att.AttendanceStatus = EmployeeAttendanceStatus.Unemployed;
        att.AbsencePoints = 1;
      } else {
        var dayTrans = employeeData.Transactions.Where(s => s.DateTime.ToDay() == att.Date).OrderBy(s => s.DateTime).ToArray();
        InternalSetAttendanceTransactions(options, operationState, att, dayTrans);

        if (employeeData.PaidVacationDays.Contains(att.Date))
          att.AttendanceStatus = EmployeeAttendanceStatus.PaidVacation;
        else if (employeeData.UnpaidVacationDays.Contains(att.Date)) {
          att.AttendanceStatus = EmployeeAttendanceStatus.UnpaidVacation;
          att.AbsencePoints = 1;
        } else if (att.Date > DateTime.Today) {
          att.AttendanceStatus = EmployeeAttendanceStatus.Upcoming;
          att.AbsencePoints = 1;
        } else if (att.InTime != null || employeeData.Employee.IsTerminalUser == false)
          att.AttendanceStatus = EmployeeAttendanceStatus.Present;
        else if (employeeData.Shift != null && employeeData.Shift.SelectIsWeekend(att.Date))
          att.AttendanceStatus = EmployeeAttendanceStatus.Weekend;
        else if (employeeData.Employee.IsTerminalUser) {
          att.AttendanceStatus = EmployeeAttendanceStatus.Absent;
          att.AbsencePoints = 1;
        } else//??!
          att.AttendanceStatus = EmployeeAttendanceStatus.Present;

        /////////////////////////////////////////////////////////////////////////////////////////////////
        // 3.d. set points
        InternalSetEmployeeAttendancePoints(att, employeeData.Shift, operationState.TransactionRules);
      }
    }

    private static void InternalSetEmployeeAttendancePoints(EmployeeAttendance att, EmployeeShift shift, EmployeeTransactionRule[] transactionRules) {
      if (transactionRules == null || transactionRules.Count() == 0) {
        return;
      }

      InternalSetEmployeeAttendanceInPoints(att, shift, transactionRules);

      InternalSetEmployeeAttendanceTimeOffPoints(att,  shift, transactionRules);

      InternalSetEmployeeAttendanceOutPoints(att, shift, transactionRules);
    }


    private static void InternalSetEmployeeAttendanceInPoints(EmployeeAttendance att, EmployeeShift shift, EmployeeTransactionRule[] transactionRules) {
      if (null == att.InTime || shift == null || att.AttendanceStatus != EmployeeAttendanceStatus.Present)
        return;

      var shiftStart = shift.SelectStartTime(att.Date);
      var fromMargin = shift.SelectFromMargin(att.Date);

      if (shiftStart != null && att.InTime > shiftStart) { // late
        var diff = att.InTime - shiftStart;

        if (diff <= fromMargin)
          return;

        var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.In && s.Time >= diff).FirstOrDefault();
        if (rule == null)
          rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.In).LastOrDefault();

        if (rule != null)
          att.InPoints = rule.Points;
      } else if (shiftStart != null && att.InTime < shiftStart && shift.SelectCalculateIsDayOvertime(att.Date)) { // overtime
        var diff = shiftStart - att.InTime;
        var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Overtime && s.Time <= diff).LastOrDefault();

        if (rule != null)
          att.InOvertimePoints = rule.Points;
      }
    }
    private static void InternalSetEmployeeAttendanceTimeOffPoints(EmployeeAttendance att, EmployeeShift shift, EmployeeTransactionRule[] transactionRules) {
      if (att.AttendanceStatus != EmployeeAttendanceStatus.Present || (shift != null && shift.SelectCalculateTimeOffs(att.Date) == false))
        return;

      // Time-Offs
      foreach (var timeOff in att.TimeOffs) {
        var diff = timeOff.InTime - timeOff.OutTime;

        var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.TimeOff && s.Time >= diff).FirstOrDefault();
        if (rule == null)
          rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.TimeOff && s.Time < diff).LastOrDefault();

        if (rule != null)
          timeOff.Points = rule.Points;
      }
    }
    private static void InternalSetEmployeeAttendanceOutPoints(EmployeeAttendance att, EmployeeShift shift, EmployeeTransactionRule[] transactionRules) {
      if (null == att.OutTime || shift == null)
        return;

      // Clock-Out
      var shiftEnd = shift.SelectEndTime(att.Date);
      var toMargin = shift.SelectToMargin(att.Date);

      if (att.AttendanceStatus == EmployeeAttendanceStatus.Present) {
        if (shiftEnd != null && att.OutTime < shiftEnd) { // early
          var diff = shiftEnd - att.OutTime;

          if (diff <= toMargin)
            return;

          var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Out && s.Time >= diff).FirstOrDefault();
          if (rule == null)
            rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Out).LastOrDefault();

          if (rule != null)
            att.OutPoints = rule.Points;
        } else if (shiftEnd != null && att.OutTime > shiftEnd && shift.SelectCalculateIsDayOvertime(att.Date)) { // overtime
          var diff = att.OutTime - shiftEnd;
          var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Overtime && s.Time <= diff).LastOrDefault();

          if (rule != null)
            att.OutPoints = rule.Points;
        }
      } else if
        ((shift.SelectCalculateIsVacationOvertime(att.Date) && (att.AttendanceStatus == EmployeeAttendanceStatus.PaidVacation || att.AttendanceStatus == EmployeeAttendanceStatus.UnpaidVacation))
        || (shift.SelectCalculateIsWeekendOvertime(att.Date) && att.AttendanceStatus == EmployeeAttendanceStatus.Weekend)) {

        var duration = att.Duration;
        var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Overtime && s.Time <= duration).LastOrDefault();

        if (rule != null)
          att.OutOvertimePoints = rule.Points;
      }
    }

    private static void InternalSetEmployeeExtraAttendancePoints(IEnumerable<EmployeeAttendance> attendance) {
      // 1. create vacation and weekend groups
      List<List<DateTime>> offGroups = new List<List<DateTime>>();

      var absences = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Absent);
      if (absences.Count() == 0)
        return;

      foreach (var att in attendance
        .Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.PaidVacation || s.AttendanceStatus == EmployeeAttendanceStatus.UnpaidVacation || s.AttendanceStatus == EmployeeAttendanceStatus.Weekend).ToArray()) {
        var grp = offGroups.SingleOrDefault(s => s.Last().Date.Day == att.Date.Day - 1);
        if (grp == null) {
          grp = new List<DateTime>();
          offGroups.Add(grp);
        }

        grp.Add(att.Date);
      }


      // 2. calculate extra absences
      foreach (var att in absences) {
        int extras = 0;

        var groups = offGroups.Where(s => s.Last().Date.Day == att.Date.Day - 1 || s.First().Date.Day == att.Date.Day + 1).ToArray();
        extras += groups.Sum(s => s.Count());

        foreach (var grp in groups)
          offGroups.Remove(grp);

        att.ExtraAbsencePoints =extras;
      }
    }


    private static void InternalSetAttendanceTransactions(EmployeeCalculateOptions options, EmployeeCalculateAttendanceState operationState, EmployeeAttendance att, EmployeeTerminalTransaction[] dayTrans) {
      // 1. load data
      var dateList = dayTrans.Select(s => s.DateTime).ToList();
      if (dateList.Count() == 0)
        return;


      Queue<DateTime> que = new Queue<DateTime>();
      List<int> toRemoveIndices = new List<int>();

      // 2. Check duplicated 
      var maxDuplication = operationState.Flags.DuplicateTransactionTime ?? operationState.Flags.DefaultDuplicateTransactionTime;
      for (int i = 1; i < dateList.Count; i++) {
        var prevDate = dateList.ElementAt(i - 1);
        var thisDate = dateList.ElementAt(i);
        if (thisDate - prevDate < maxDuplication)
          toRemoveIndices.Add(i);
      }

      // 3. remove duplications and load into a FIFO list
      for (int i = 0; i < dateList.Count; i++) {
        if (toRemoveIndices.Contains(i) == false) {
          que.Enqueue(dateList.ElementAt(i));
        }
      }

      // 4. set options
      if (que.Count > 0) // clock-in
        att.InTime = que.Dequeue().TimeOfDay.ToMinutes();

      while (true) {
        if (que.Count >= 2)
          att.TimeOffs.Add(new EmployeeAttendanceTimeOff() {
            OutTime = que.Dequeue().TimeOfDay.ToMinutes(),
            InTime = que.Dequeue().TimeOfDay.ToMinutes(),
          });
        else
          break;
      }

      if (que.Count > 0) // clock-out
        att.OutTime = que.Dequeue().TimeOfDay.ToMinutes();
    }

  }
}