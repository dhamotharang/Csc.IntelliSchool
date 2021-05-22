using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    public EmployeeAttendance UpdateEmployeeAttendance(EmployeeAttendance userItem) {
      using (var ent = ServiceManager.CreateModel()) {
        var dbItem = ent.EmployeeAttendance.Include("TimeOffs").SingleOrDefault(s => s.AttendanceID == userItem.AttendanceID);
        ent.Entry(dbItem).CurrentValues.SetValues(userItem);
        ent.UpdateChildEntities( dbItem.TimeOffs.ToArray(), userItem.TimeOffs.ToArray(), (a, b) => a.TimeOffID == b.TimeOffID);

        dbItem.IsEdited = true;

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Update, typeof(EmployeeAttendance), userItem.AttendanceID.PackArray());

        ent.SaveChanges();

        return ent.EmployeeAttendance.Include("TimeOffs").SingleOrDefault(s => s.AttendanceID == userItem.AttendanceID);
      }
    }

    public EmployeeAttendance[] GetSingleEmployeeAttendance(int employeeId, DateTime month, EmployeeRecalculateFlags flags) {
      using (var ent = ServiceManager.CreateModel()) {

        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(flags);
        options.Model = ent;
        options.Month = month;

        return InternalGetEmployeeAttendance(options, new int[] { employeeId });
      }
    }

    public void RecalculateEmployeeMonthly(DateTime month, int[] listIds, EmployeeRecalculateFlags flags) {
      int[] employeeIds = new int[] { };
      using (var ent = ServiceManager.CreateModel()) {
        employeeIds = ent.Employees.Query(EmployeeDataFilter.None, month, listIds).Select(s => s.EmployeeID).ToArray();
      }

      if (employeeIds.Count() == 0)
        return;

      if (flags.HasFlag(EmployeeRecalculateFlags.Attendance))
        RecalculateEmployeeAttendanceByEmployees(month, employeeIds, flags);

      if (flags.HasFlag(EmployeeRecalculateFlags.Earning))
        RecalculateEmployeeEarnings(month, employeeIds, flags);
    }


    public void RecalculateEmployeeAttendance(DateTime month, EmployeeRecalculateFlags flags) {
      flags = flags | EmployeeRecalculateFlags.Attendance;

      using (var ent = ServiceManager.CreateModel(false)) {
        var employeeIds = ent.Employees.Query(EmployeeDataFilter.None, month, null, null).Select(s => s.EmployeeID).ToArray();

        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(flags);
        options.Model = ent;
        options.Flags = InternalGetHumanResourcesFlags(ent);
        options.Month = month;

        InternalGetEmployeeAttendance(options, employeeIds);
      }
    }
    public void RecalculateEmployeeAttendanceByTerminal(DateTime month, int terminalId, EmployeeRecalculateFlags flags) {
      flags = flags | EmployeeRecalculateFlags.Attendance;

      using (var ent = ServiceManager.CreateModel(false)) {
        var qry = ent.Employees.Query(EmployeeDataFilter.None, month, null, null);

        if (terminalId == 0) // terminals only
          qry = qry.Where(s => s.TerminalID != null && s.TerminalUserID != null);
        else
          qry = qry.Where(s => s.TerminalID == terminalId && s.TerminalUserID != null);

        var employeeIds = qry.Select(s => s.EmployeeID).ToArray();

        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(flags);
        options.Model = ent;
        options.Flags = InternalGetHumanResourcesFlags(ent);
        options.Month = month;

        InternalGetEmployeeAttendance(options, employeeIds);
      }
    }
    public void RecalculateEmployeeAttendanceByEmployees(DateTime month, int[] employeeIds, EmployeeRecalculateFlags flags) {
      flags = flags | EmployeeRecalculateFlags.Attendance;

      using (var ent = ServiceManager.CreateModel(false)) {
        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(flags);
        options.Model = ent;
        options.Flags = InternalGetHumanResourcesFlags(ent);
        options.Month = month;

        InternalGetEmployeeAttendance(options, employeeIds);
      }
    }



    public EmployeeAttendanceObject[] GetEmployeeAttendance(int? branchId, int? departmentId, int? positionId, int[] listIds, DateTime month, EmployeeRecalculateFlags flags) {
      using (var ent = ServiceManager.CreateModel()) {
        var qry = ent.Employees.Query(EmployeeDataFilter.Employment, month, listIds, null);
        if (branchId != null)
          qry = qry.Where(s => s.BranchID == branchId);
        if (departmentId != null)
          qry = qry.Where(s => s.DepartmentID == departmentId);
        if (positionId != null)
          qry = qry.Where(s => s.PositionID == positionId);

        var employees = qry.ToArray();

        EmployeeRecalculateOptionFlags options = new EmployeeRecalculateOptionFlags(flags);
        options.Model = ent;
        options.Month = month;

        var allAttendance = InternalGetEmployeeAttendance(options, employees.Select(s => s.EmployeeID).ToArray());

        return employees.Select(emp =>
          new EmployeeAttendanceObject() {
            Employee = emp,
            Attendance = allAttendance.Where(s => s.EmployeeID == emp.EmployeeID).ToArray()
          }).ToArray();
      }
    }


    private EmployeeAttendance[] InternalGetEmployeeAttendance(EmployeeRecalculateOptionFlags options, int[] employeeIds) {
      var attendanceList =
        options.Model.EmployeeAttendance.Include("TimeOffs").Where(s => employeeIds.Contains(s.EmployeeID) && s.Date >= options.StartDate && s.Date <= options.EndDate).ToList();

      // remove duplicates
      Array.ForEach(attendanceList.GroupBy(s => s.EmployeeID).ToArray(),
        empGrp => {
          foreach (var dateGrp in empGrp.GroupBy(s => s.Date).Where(s => s.Count() > 1).ToArray()) {
            foreach (var itm in dateGrp.Skip(1)) {
              attendanceList.Remove(itm);
              options.Model.EmployeeAttendance.Remove(itm);
            }
          }
        });

      int[] recalcEmployeeIdList = options.CalculateAttendance ? employeeIds :
        attendanceList.GroupBy(s => s.EmployeeID).Where(s => s.Count() != options.EndDate.Day).Select(s => s.Key).ToArray()
        .Concat(employeeIds.Except(attendanceList.Select(s => s.EmployeeID))).Distinct().ToArray();

      if (recalcEmployeeIdList.Count() > 0) {
        var employees = options.Model.Employees.Include(EmployeeDataFilter.ShiftOverridesTerminal)
          .Where(s => recalcEmployeeIdList.Contains(s.EmployeeID)).ToArray();
        var departmentIds = employees.Where(s => s.DepartmentID != null).Select(s => s.DepartmentID.Value).Distinct().ToArray();

        if (options.Flags == null)
          options.Flags = InternalGetHumanResourcesFlags(options.Model);

        var terminalIPs = employees.Where(s => s.IsTerminalUser).Select(s => s.Terminal.IP).Distinct().ToArray();
        var terminalUserIDs = employees.Where(s => s.IsTerminalUser).Select(s => s.TerminalUserID).Distinct().ToArray();
        var terminalTransactions = new EmployeeTerminalTransaction[] { };
        if (terminalIPs.Count() > 0) {
          var qry = options.Model.EmployeeTerminalTransactions.Where(s => s.DateTime >= options.StartDate && s.DateTime <= options.EndTime).AsQueryable();
          if (terminalIPs.Count() == 1 && terminalUserIDs.Count() == 1) {
            var tmpTerminalIp = terminalIPs.First();
            var tmpUserId = terminalUserIDs.First();
            terminalTransactions = qry.Where(s => s.TerminalIP == tmpTerminalIp && s.UserID == tmpUserId).ToArray();
          } else
            terminalTransactions = qry.Where(s => terminalIPs.Contains(s.TerminalIP) || terminalUserIDs.Contains(s.UserID)).ToArray();
        }
        var transactionRules = options.Model.EmployeeTransactionRules.ToArray();

        var vacations = options.Model.EmployeeVacations.Query(employeeIds, options.StartDate, options.EndDate).ToArray();
        var departmentVacations = departmentIds.Count() > 0 ? options.Model.EmployeeDepartmentVacations.Query(departmentIds, options.StartDate, options.EndDate).ToArray() : new EmployeeDepartmentVacation[] { };

        foreach (var emp in employees) {
          InternalCalculateSingleEmployeeAttendance(options, emp,
            attendanceList,
            terminalTransactions, transactionRules, vacations, departmentVacations);
        }

        options.Model.Logger.LogDatabase(ServiceManager.GetCurrentUser(options.Model), SystemLogDataAction.Calculate, typeof(EmployeeAttendance), null,
          new SystemLogDataEntry() { EmployeeIDs = employees.Select(s => s.EmployeeID).ToArray(), EmployeeRecalculateFlags = options.OptionFlags, Month = options.Month
          });

        options.Model.SaveChanges();
      }

      return attendanceList.OrderBy(s => s.EmployeeID).ThenBy(s => s.Date).ToArray();
    }

    private static EmployeeAttendance[] InternalCalculateSingleEmployeeAttendance(EmployeeRecalculateOptionFlags options,
      Employee emp,
      List<EmployeeAttendance> allAttendance,
      EmployeeTerminalTransaction[] allTransactions,
      EmployeeTransactionRule[] transactionRules,
      EmployeeVacation[] allVacations,
      EmployeeDepartmentVacation[] allDepartmentVacations) {

      //////////////////////////////////////////////////////////////////////////
      // 1. Check if not already loaded (batch loading)
      if (options.Flags == null)
        options.Flags = InternalGetHumanResourcesFlags(options.Model);

      if (null == allTransactions && emp.IsTerminalUser) {
        allTransactions = options.Model.EmployeeTerminalTransactions.Where(s => s.TerminalIP == emp.Terminal.IP && s.UserID == emp.TerminalUserID && s.DateTime >= options.StartDate && s.DateTime <= options.EndTime)
          .OrderBy(s => s.DateTime).ThenBy(s => s.TransactionID).ToArray();
      } else if (allTransactions == null)
        allTransactions = new EmployeeTerminalTransaction[] { };

      if (null == transactionRules && emp.IsTerminalUser && emp.ShiftID != null) {
        transactionRules = options.Model.EmployeeTransactionRules.OrderBy(s => s.Type).ThenBy(s => s.Time).ThenBy(s => s.RuleID).ToArray();
      }

      if (null == allVacations) {
        allVacations = options.Model.EmployeeVacations.Query(emp.EmployeeID, options.StartDate, options.EndDate).ToArray();
      }

      if (null == allDepartmentVacations && emp.DepartmentID != null) {
        allDepartmentVacations = options.Model.EmployeeDepartmentVacations.Query(emp.DepartmentID.Value, options.StartDate, options.EndDate).ToArray();
      } else if (null == allDepartmentVacations)
        allDepartmentVacations = new EmployeeDepartmentVacation[] { };

      if (null == allAttendance) {
        allAttendance = options.Model.EmployeeAttendance.Include("TimeOffs").Where(s => s.EmployeeID == emp.EmployeeID && s.Date >= options.StartDate && s.Date <= options.EndDate).OrderBy(s => s.Date).ToList();
      }

      //////////////////////////////////////////////////////////////////////////
      // 2. load specific employee items
      //DayOfWeek[] weekends = emp.Shift != null ? emp.Shift.GetWeekends() : new DayOfWeek[] { };
      var transactions = emp.IsTerminalUser ? allTransactions.Where(s => s.TerminalIP == emp.Terminal.IP && s.UserID == emp.TerminalUserID).ToArray() : new EmployeeTerminalTransaction[] { };
      var attendance = allAttendance.Where(s => s.EmployeeID == emp.EmployeeID).ToList();
      var vacationDays =
        allVacations.Where(s => s.EmployeeID == emp.EmployeeID).SelectMany(s => s.GetDays())
        .Concat(allDepartmentVacations.Where(s => s.Departments.Where(x => x.DepartmentID == emp.DepartmentID).Count() > 0).SelectMany(s => s.GetDays()))
        .Where(s => s.Date >= options.StartDate && s.Date <= options.EndDate).Distinct().ToArray();

      //////////////////////////////////////////////////////////////////////////
      // 3. Calculate 
      DateTime date = options.StartDate.ToDay();
      while (date <= options.EndDate.ToDay()) {
        //////////////////
        // 3.a. Get date attendance
        var dateAtt = attendance.Where(s => s.Date == date);
        if (dateAtt.Count() > 1) { // second check
          foreach (var attObj in dateAtt.Skip(1).ToArray()) {
            options.Model.EmployeeAttendance.Remove(attObj);
            attendance.Remove(attObj);
            allAttendance.Remove(attObj);
          }
        }
        var att = dateAtt.FirstOrDefault();

        //////////////////
        // 3.b. if not available, create one
        if (att == null) {
          att = new EmployeeAttendance() {
            Date = date,
            EmployeeID = emp.EmployeeID,
          };

          options.Model.EmployeeAttendance.Add(att);
          attendance.Add(att);
          allAttendance.Add(att);
        } else if
          ((options.CalculateAttendance == false) || //available and recalculate == false
          (options.CalculatedEditedAttendance == false && att.IsEdited) ||  // exclude edited, item is edited
          (options.CalculatedLockedAttendance == false && att.IsLocked))  // exclude locked, item is locked
          continue;
        else
          options.Model.SetModified(att);

        ////////////////////////////////////////////
        // recalculate or a new item
        att.InTime = att.OutTime = null;
        att.InPoints = att.OutPoints = null;
        att.OvertimePoints = null;
        att.AbsencePoints = null;
        att.ExtraAbsencePoints = null;
        att.IsLocked = false;
        att.IsEdited = false;

        foreach (var timeOff in att.TimeOffs.ToArray()) {
          options.Model.EmployeeAttendanceTimeOffs.Remove(timeOff);
        }

        //////////////////////////////////////////////
        // 3.c. set transactions and status

        if (att.Date < emp.HireDate || (emp.TerminationDate != null && emp.TerminationDate < att.Date))
          att.AttendanceStatus = EmployeeAttendanceStatus.Unemployed;
        else {
          var dayTrans = transactions.Where(s => s.DateTime.ToDay() == date).OrderBy(s => s.DateTime).ToArray();
          InternalSetEmployeeAttendanceTransactions(options, att, dayTrans);

          if (att.InTime != null)
            att.AttendanceStatus = EmployeeAttendanceStatus.Present;
          else if (vacationDays.Contains(date))
            att.AttendanceStatus = EmployeeAttendanceStatus.Vacation;
          else if (emp.Shift != null && emp.Shift.IsWeekend(date))
            att.AttendanceStatus = EmployeeAttendanceStatus.Weekend;
          else if (emp.IsTerminalUser == false)
            att.AttendanceStatus = EmployeeAttendanceStatus.Present;
          else if (att.Date > DateTime.Today)
            att.AttendanceStatus = EmployeeAttendanceStatus.Upcoming;
          else if (emp.IsTerminalUser) {
            att.AttendanceStatus = EmployeeAttendanceStatus.Absent;
            att.AbsencePoints = 1;
          } else//??!
            att.AttendanceStatus = EmployeeAttendanceStatus.Present;

          //////////////////
          // 3.d. set points
          InternalSetEmployeeAttendancePoints(att, emp.Shift, transactionRules);
        }

        //////////////////
        // 3.d. move forward
        date = date.AddDays(1);
      }

      attendance = attendance.OrderBy(s => s.Date).ToList();

      InternalSetEmployeeExtraAttendancePoints(attendance);

      return attendance.ToArray();
    }

    private static void InternalSetEmployeeAttendanceTransactions(EmployeeRecalculateOptionFlags options, EmployeeAttendance att, EmployeeTerminalTransaction[] dayTrans) {
      // 1. load data
      var dateList = dayTrans.Select(s => s.DateTime).ToList();
      if (dateList.Count() == 0)
        return;


      Queue<DateTime> que = new Queue<DateTime>();
      List<int> toRemoveIndices = new List<int>();

      // 2. Check duplicated 
      var maxDuplication = options.Flags.DuplicateTransactionTime ?? new TimeSpan(00, 05, 00);
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
        att.InTime = que.Dequeue().TimeOfDay;

      while (true) {
        if (que.Count >= 2)
          att.TimeOffs.Add(new EmployeeAttendanceTimeOff() {
            OutTime = que.Dequeue().TimeOfDay,
            InTime = que.Dequeue().TimeOfDay,
          });
        else
          break;
      }

      if (que.Count > 0) // clock-out
        att.OutTime = que.Dequeue().TimeOfDay;
    }
    private static void InternalSetEmployeeAttendancePoints(EmployeeAttendance att, EmployeeShift shift, EmployeeTransactionRule[] transactionRules) {
      if (transactionRules == null || transactionRules.Count() == 0) {
        return;
      }

      // Clock-In
      if (null != att.InTime && shift != null) { // in 'if' statement just for the sake of readness
        var shiftStart = shift.GetStart(att.Date);
        if (shiftStart != null && att.InTime > shiftStart) {
          var diff = att.InTime - shiftStart;
          var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.In && s.Time >= diff).FirstOrDefault();
          if (rule == null)
            rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.In).LastOrDefault();

          if (rule != null)
            att.InPoints = rule.Points;
        }
      }

      // Time-Offs
      foreach (var timeOff in att.TimeOffs) {
        var diff = att.InTime - att.OutTime;

        var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.TimeOff && s.Time >= diff).FirstOrDefault();
        if (rule == null)
          rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.TimeOff).LastOrDefault();

        if (rule != null)
          timeOff.Points = rule.Points;
      }

      // Clock-Out
      if (null != att.OutTime && shift != null) { // in 'if' statement just for the sake of readness
        var shiftEnd = shift.GetEnd(att.Date);
        if (shiftEnd != null && att.OutTime < shiftEnd) {
          var diff = shiftEnd - att.OutTime;
          var rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Out && s.Time >= diff).FirstOrDefault();
          if (rule == null)
            rule = transactionRules.Where(s => s.RuleType == EmployeeTransactionRuleType.Out).LastOrDefault();

          if (rule != null)
            att.OutPoints = rule.Points;
        }
      }
    }
    private static void InternalSetEmployeeExtraAttendancePoints(IEnumerable<EmployeeAttendance> attendance) {
      // 1. create vacation and weekend groups
      List<List<DateTime>> offGroups = new List<List<DateTime>>();

      var absences = attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Absent);
      if (absences.Count() == 0)
        return;

      foreach (var att in attendance.Where(s => s.AttendanceStatus == EmployeeAttendanceStatus.Vacation || s.AttendanceStatus == EmployeeAttendanceStatus.Weekend).ToArray()) {
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

        att.ExtraAbsencePoints = extras;
      }
    }
  }

}
