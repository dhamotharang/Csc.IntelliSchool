using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {

  [Flags]
  public enum EmployeeLoanIncludes {
    None = 0,
    [DataInclude("Installments")]
    Installments = 1 << 0,

    [DataInclude("Employee")]
    Employee = 1 << 1,
  }

}