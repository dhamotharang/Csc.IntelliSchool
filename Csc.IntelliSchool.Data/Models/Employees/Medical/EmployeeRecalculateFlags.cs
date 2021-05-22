using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeRecalculateFlags {
    None = 0,

    Attendance = 1 << 0,
    EditedAttendance = Attendance | (1 << 1),
    LockedAttendance = Attendance | (1 << 2),

    Earning = 1 << 10,
    EarningSalariesOnly = Earning | (1 << 11),
    EditedEarning = Earning | (1 << 12)
  }
}