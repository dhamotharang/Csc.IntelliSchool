using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeAttendanceIncludes {
    None = 0,
    [DataInclude("TimeOffs")]
    TimeOffs = 1 << 0,
  }
}