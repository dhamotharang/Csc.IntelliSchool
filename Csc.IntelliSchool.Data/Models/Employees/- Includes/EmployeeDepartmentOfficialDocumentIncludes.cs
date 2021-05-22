using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeDepartmentOfficialDocumentIncludes {
    None = 0,
    [DataInclude("Type")]
    Type = 1 << 0,
  }
}