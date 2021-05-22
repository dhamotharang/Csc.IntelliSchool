using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Assets.Helpers {
  public enum EmployeeAttendanceStyleType {
    Absence,
    Attendance,
    Overtime,
    TimeOffs
  }

  public class EmployeeAttendanceStyleSelector : StyleSelector {
    public Style NegativeStyle { get; set; }
    public Style PositiveStyle { get; set; }
    public Style NeutralStyle { get; set; }
    public EmployeeAttendanceStyleType Type { get; set; }

    public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container) {
      var cell = container as Telerik.Windows.Controls.GridView.GridViewCell;
      var col = cell.Column;

      EmployeeAttendance att = item as EmployeeAttendance;
      if (att == null)
        return null;

      decimal? points = null;

      if (Type == EmployeeAttendanceStyleType.Absence)
        points = att.TotalAbsencePoints;
      else if (Type == EmployeeAttendanceStyleType.Attendance)
        points = att.TotalAttendancePoints;
      else if (Type == EmployeeAttendanceStyleType.Overtime)
        points = att.TotalOvertimePoints;
      else if (Type == EmployeeAttendanceStyleType.TimeOffs)
        points = att.TotalTimeOffPoints;


      if (points != null) {
        if (points != 0 && Type == EmployeeAttendanceStyleType.Overtime)
          return PositiveStyle;
        else if (points != 0)
          return NegativeStyle;
      }

      return NeutralStyle;
    }
  }
}