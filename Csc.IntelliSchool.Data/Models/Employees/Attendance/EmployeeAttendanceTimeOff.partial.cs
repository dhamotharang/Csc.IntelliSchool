
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using Csc.Components.Common;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeAttendanceTimeOff {


    [IgnoreDataMember]
    [NotMapped]
    public DateTime? InDateTime {
      get {
        return InTime.ToDateTime();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public DateTime? OutDateTime {
      get {
        return OutTime.ToDateTime();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public TimeSpan? Duration {
      get {
        return InTime - OutTime;
      }
    }
  }

}