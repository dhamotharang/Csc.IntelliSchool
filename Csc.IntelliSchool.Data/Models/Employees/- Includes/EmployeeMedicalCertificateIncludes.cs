using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  public enum EmployeeMedicalCertificateIncludes {
    None = 0,
    [DataInclude("Employees")]
    Employees = 1 << 0,
    [DataInclude("Dependants.Employee")]
    Dependants = 1 << 1,
    [DataInclude("Program.Provider")]
    Program = 1 << 2,
    [DataInclude("Program.Concessions")]
    ProgramConcessions = 1 << 3,
    [DataInclude("Program.Rates")]
    ProgramRates = 1 << 4,
    ProgramDetails = ProgramConcessions | ProgramRates 
  }
}