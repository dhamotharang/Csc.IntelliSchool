 using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeDepartmentVacationIncludes {
    None = 0,
    [DataInclude("Departments.Department")]
    Department = 1 << 0,
  }

}