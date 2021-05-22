using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Csc.Components.Common;

namespace Csc.IntelliSchool.Data {
  
  public class EmployeeCalculateOptions {
    #region Options
    public EmployeeRecalculateFlags OptionFlags { get; set; }
    public bool CalculateAttendance { get { return OptionFlags.HasFlag(EmployeeRecalculateFlags.Attendance); } }
    public bool CalculatedEditedAttendance { get { return OptionFlags.HasFlag(EmployeeRecalculateFlags.EditedAttendance); } }
    public bool CalculatedLockedAttendance { get { return OptionFlags.HasFlag(EmployeeRecalculateFlags.LockedAttendance); } }
    public bool CalculateEarning { get { return OptionFlags.HasFlag(EmployeeRecalculateFlags.Earning); } }
    public bool CalculateEditedEarning { get { return OptionFlags.HasFlag(EmployeeRecalculateFlags.EditedEarning); } }
    public bool CalculateEarningSalariesOnly { get { return OptionFlags.HasFlag(EmployeeRecalculateFlags.EarningSalariesOnly); } }
    #endregion
    
    #region Date Range
    private DateTime _month;
    public DateTime Month {
      get { return _month; }
      set {
        value = value.ToMonth();
        _month = value;
        StartDate = _month;
        EndDate = _month.ToMonthEnd();
        EndTime = EndDate.ToDayEnd();
      }
    }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime EndTime { get; private set; }
    #endregion

    public EmployeeCalculateOptions() {
      OptionFlags = EmployeeRecalculateFlags.None;
    }
    public EmployeeCalculateOptions(EmployeeRecalculateFlags flags) {
      OptionFlags = flags;
    }
  }
}