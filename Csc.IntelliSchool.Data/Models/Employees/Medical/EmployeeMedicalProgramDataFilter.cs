using Csc.Components.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {

  [Flags]
  public enum EmployeeMedicalProgramDataFilter {
    None,
    [DataInclude("Provider")]
    Provider = 1 << 0,
    [DataInclude("Rates", "Concessions")]
    Rates = 1 << 1,
    [DataInclude]
    Info = 1 << 2,
    Full = Provider | Rates | Info
  }
}