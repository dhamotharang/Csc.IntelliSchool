using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeShiftOverrideIncludes {
    None = 0,
    [DataInclude("Shift")]
    Shift = 1 << 0,
    [DataInclude("Type")]
    Type = 1 << 1,

    All = Shift | Type
  }
}