using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeAllowanceIncludes {
    None = 0,
    [DataInclude("Type")]
    Type = 1 << 0,

    [DataInclude("Employee")]
    Employee = 1 << 1,

  }
}