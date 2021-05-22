using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeAttendance : IChildrenObject {
    [IgnoreDataMember]
    [NotMapped]
    public object[] ChildObjects { get { return TimeOffs != null ? TimeOffs.ToArray() : new object[] { }; } }

    [IgnoreDataMember]
    [NotMapped]
    public DateTime? InDateTime {
      get {
        if (InTime == null)
          return null;

        return DateTime.Today.Add(InTime.Value);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public DateTime? OutDateTime {
      get {
        if (OutTime == null)
          return null;

        return DateTime.Today.Add(OutTime.Value);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public TimeSpan? Duration {
      get {
        return InTime != null && OutTime != null ? 
          OutTime - InTime  - (TimeOffDuration ?? TimeSpan.Zero)
          : null;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public TimeSpan? TimeOffDuration { get { return TimeOffs != null ? TimeOffs.Select(s => s.Duration).Sum() : new TimeSpan?(); } }

    [IgnoreDataMember]
    [NotMapped]
    public int? TotalAbsencePoints {
      get {
        var val = (this.AbsencePoints ?? 0) + (this.ExtraAbsencePoints ?? 0);
        return val != 0 ? val : new int?();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public decimal? TotalAttendancePoints {
      get {
        var val = (InPoints ?? 0) + (OutPoints ?? 0);

        return val != 0 ? val : new decimal?();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public decimal? TotalTimeOffPoints {
      get {
        var points = this.TimeOffs != null ? this.TimeOffs.Sum(s => s.Points ?? 0) : new decimal?();
        if (points != null && points != 0)
          return points;
        else
          return null;
      }
    }


    [IgnoreDataMember]
    [NotMapped]
    public decimal? TotalOvertimePoints {
      get {
        var val = (InOvertimePoints ?? 0) + (OutOvertimePoints ?? 0);

        return val != 0 ? val : new decimal?();
      }
    }

  
    [IgnoreDataMember]
    [NotMapped]
    public EmployeeAttendanceStatus AttendanceStatus {
      get {
        EmployeeAttendanceStatus status = EmployeeAttendanceStatus.Unknown;

        if (this.Status == null)
          return status;

        Enum.TryParse(this.Status, true, out status);

        return status;
      }
      set {
        Status = value.ToString();
      }
    }

    public void ClearData() {
      this.InTime = this.OutTime = null;
      this.InOvertimePoints = this.OutOvertimePoints = null;
      this.InPoints = this.OutPoints = null;
      this.AbsencePoints = this.ExtraAbsencePoints = null;
      this.IsLocked = false;
      this.IsEdited = false;
    }
  }

}