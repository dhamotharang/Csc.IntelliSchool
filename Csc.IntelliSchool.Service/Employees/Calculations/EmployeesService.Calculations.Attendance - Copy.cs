using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    private static EmployeeAttendance[] InternalCalculateSingleEmployeeAttendance(
      Employee emp,
      EmployeeCalculateOptions options,
      EmployeeCalculateState operationState) {

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 1. ensure that required data is loaded
      InternalLoadCalculateState(emp.PackArray(), options, operationState );

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 2. load specific employee items
      //DayOfWeek[] weekends = emp.Shift != null ? emp.Shift.GetWeekends() : new DayOfWeek[] { };
      var transactions = emp.IsTerminalUser ?
        operationState.Transactions.Where(s => s.TerminalIP == emp.Terminal.IP && s.UserID == emp.TerminalUserID).ToArray() :
        new EmployeeTerminalTransaction[] { };
      var attendance = operationState.Attendance.Where(s => s.EmployeeID == emp.EmployeeID).ToList();

      var vacations = operationState.Vacations.Where(s => s.EmployeeID == emp.EmployeeID).ToArray();
      var unpaidVacationDays =
        vacations.Where(s => s.IsPaid == false).SelectMany(s => s.GetDays())
        .Where(s => s.Date >= options.StartDate && s.Date <= options.EndDate).Distinct().ToArray();
      var paidVacationDays =
        vacations.Where(s => s.IsPaid == true).SelectMany(s => s.GetDays())
        .Concat((emp.DepartmentID != null ? operationState.DepartmentVacations : new EmployeeDepartmentVacation[] { })
        .Where(s => s.Departments.Where(x => x.DepartmentID == emp.DepartmentID).Count() > 0).SelectMany(s => s.GetDays()))
        .Where(s => s.Date >= options.StartDate && s.Date <= options.EndDate).Distinct().ToArray();
      paidVacationDays = paidVacationDays.Except(unpaidVacationDays).ToArray();

      /////////////////////////////////////////////////////////////////////////////////////////////////
      // 3. Calculate 
      DateTime date = options.StartDate.ToDay();
      while (date <= options.EndDate.ToDay()) {
        //////////////////
        // 3.a. Get date attendance
        var dateAtt = attendance.Where(s => s.Date == date);
        if (dateAtt.Count() > 1) { // second check
          foreach (var attObj in dateAtt.Skip(1).ToArray()) {
            operationState.Model.EmployeeAttendance.Remove(attObj);
            attendance.Remove(attObj);
            operationState.Attendance.Remove(attObj);
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

          operationState.Model.EmployeeAttendance.Add(att);
          attendance.Add(att);
          operationState.Attendance.Add(att);
        } else if // attendance not null
          ((options.CalculateAttendance == false) || //available and recalculate == false
          (options.CalculatedEditedAttendance == false && att.IsEdited) ||  // exclude edited, item is edited
          (options.CalculatedLockedAttendance == false && att.IsLocked))  // exclude locked, item is locked
          continue;
        else
          operationState.Model.SetModified(att);

        ////////////////////////////////////////////
        // recalculate or a new item
        att.ClearData();
        foreach (var timeOff in att.TimeOffs.ToArray()) {
          operationState.Model.EmployeeAttendanceTimeOffs.Remove(timeOff);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        // 3.c. set transactions and status

        if (att.Date < emp.HireDate || (emp.TerminationDate != null && emp.TerminationDate < att.Date))
          att.AttendanceStatus = EmployeeAttendanceStatus.Unemployed;
        else {
          var dayTrans = transactions.Where(s => s.DateTime.ToDay() == date).OrderBy(s => s.DateTime).ToArray();
          InternalSetEmployeeAttendanceTransactions(options, att, dayTrans);

          if (att.InTime != null)
            att.AttendanceStatus = EmployeeAttendanceStatus.Present;
          else if (paidVacationDays.Contains(date))
            att.AttendanceStatus = EmployeeAttendanceStatus.PaidVacation;
          else if (unpaidVacationDays.Contains(date))
            att.AttendanceStatus = EmployeeAttendanceStatus.UnpaidVacation;
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

          /////////////////////////////////////////////////////////////////////////////////////////////////
          // 3.d. set points
          InternalSetEmployeeAttendancePoints(att, emp.Shift, operationState.TransactionRules);
        }

        //////////////////
        // 3.d. move forward
        date = date.AddDays(1);
      }

      attendance = attendance.OrderBy(s => s.Date).ToList();

      InternalSetEmployeeExtraAttendancePoints(attendance);

      return attendance.ToArray();
    }

    
  }
}