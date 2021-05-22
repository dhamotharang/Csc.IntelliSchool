using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  public enum EmployeeLoanInstallmentIncludes {
    None = 0,
    [DataInclude("Loan")]
    Loan = 1 << 0
  }
}