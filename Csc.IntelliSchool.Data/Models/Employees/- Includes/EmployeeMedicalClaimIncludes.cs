using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeMedicalClaimIncludes {
    None = 0,
    [DataInclude("Status")]
    Status = 1 << 0,
    [DataInclude("Type")]
    Type = 1 << 1,
    [DataInclude("Employee.Person")]
    Employee = 1 << 2,
    [DataInclude("Employee.Department", "Employee.Position")]
    EmployeePosition = 1 << 3,
    [DataInclude("Dependant.Person")]
    Dependant  = 1 << 4,
    All = Status |  Type | Employee | EmployeePosition | Dependant
  }
}