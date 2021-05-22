using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Windows.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Assets.Helpers {
  public class EmployeeAttendanceValueConverter : IValueConverter {
    public bool Detailed { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      EmployeeAttendance att = value as EmployeeAttendance;
      if (value == null || parameter == null)
        return null;

      if (att.AttendanceStatus == EmployeeAttendanceStatus.Absent)
        return "A";
      else if (att.AttendanceStatus == EmployeeAttendanceStatus.Unemployed || att.AttendanceStatus == EmployeeAttendanceStatus.Upcoming)
        return "U";
      else if (att.AttendanceStatus == EmployeeAttendanceStatus.Unknown)
        return null;
      else if (att.AttendanceStatus == EmployeeAttendanceStatus.PaidVacation)
        return "PV";
      else if (att.AttendanceStatus == EmployeeAttendanceStatus.UnpaidVacation)
        return "UV";
      else if (att.AttendanceStatus == EmployeeAttendanceStatus.Weekend)
        return "W";
      else { // present
        if (parameter.ToString() == "In" ) {
          if (att.InTime == null)
            return "P";

          if (Detailed || att.InPoints  > 0)
            return att.InDateTime.Value.ToString("HH:mm");
          else // short version or att.InPoints == null
            return "P";
        } else if (parameter.ToString() == "Out") {
          if (att.OutTime == null)
            return "P";

          if (Detailed || att.OutPoints > 0)
            return att.OutDateTime.Value.ToString("HH:mm");
          else // short version or att.OutPoOutts == null
            return "P";
        } else if (parameter.ToString() == "TO") {
          if (att.TimeOffs == null || att.TimeOffs.Count == 0)
            return null;

          if (Detailed || att.TotalTimeOffPoints > 0)
            return att.TimeOffDuration.ToString();
          else
            return null;
        }
      }

      return null;
      
    }



    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
