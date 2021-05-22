using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum EmployeeIncludes {
    None = 0,

    [DataInclude("Person")]
    Person = 1 << 0, 
    [DataInclude("Person.Religion", "Person.Nationality")]
    Personal = Person | (1 << 1),
    [DataInclude("Person.EducationDegree", "Person.EducationField" )]
    Education = Person | (1 << 2),
    [DataInclude("Person.Contact.Addresses", "Person.Contact.Numbers")]
    Contact = Person | (1 << 3), 

    [DataInclude("Branch", "Department", "Position", "List")]
    Employment = 1 << 4,

    [DataInclude("Shift")]
    Shift = 1 << 5, 
    [DataInclude("Shift.Overrides")]
    ShiftOverrides = Shift | (1 << 6), 
    [DataInclude("Terminal")]
    Terminal = 1 << 7,

    ShiftTerminal = Shift | Terminal,
    ShiftOverridesTerminal = ShiftOverrides | Terminal,

    [DataInclude("BankAccounts")]
    Bank = 1 << 8, 
    [DataInclude("MedicalCertificate.Program.Provider")]
    Medical = 1 << 9,  
    [DataInclude("SocialInsurance")]
    SocialInsurance = 1 << 10,
    [DataInclude("Salary")]
    Salary = 1 << 11,

    [DataInclude("Dependants")]
    Dependants = 1 << 12,
    [DataInclude("Dependants.Person")]
    DependantsPerson = Dependants | (1 << 13 ),
    [DataInclude("Dependants.Person.Religion", "Dependants.Person.Nationality")]
    DependantsPersonal = DependantsPerson | (1 << 14 ),
    [DataInclude("Dependants.MedicalCertificate")]
    DependantsMedical = Dependants | (1 << 15 ),

    [DataInclude("OfficialDocuments")]
    OfficialDocuments = 1 << 16,


    EmployeeList = EmployeeIncludes.Personal |  EmployeeIncludes.Contact | 
      EmployeeIncludes.Employment | EmployeeIncludes.Shift | EmployeeIncludes.Terminal,
    EmployeeListDetails = EmployeeIncludes.Personal | EmployeeIncludes.Contact |
      EmployeeIncludes.Employment | EmployeeIncludes.Shift | EmployeeIncludes.Terminal | 
      EmployeeIncludes.Education | EmployeeIncludes.DependantsPersonal,
    EmployeeListSalaries = EmployeeIncludes.Personal | EmployeeIncludes.Contact |
      EmployeeIncludes.Employment | EmployeeIncludes.Shift | EmployeeIncludes.Terminal |
      EmployeeIncludes.Salary,
    EmployeeListBank = EmployeeIncludes.Personal | EmployeeIncludes.Contact |
      EmployeeIncludes.Employment | EmployeeIncludes.Shift | EmployeeIncludes.Terminal |
      EmployeeIncludes.Bank,
    EmployeeListFull = EmployeeIncludes.Personal | EmployeeIncludes.Contact |
      EmployeeIncludes.Employment | EmployeeIncludes.Shift | EmployeeIncludes.Terminal |
      EmployeeIncludes.Education | EmployeeIncludes.Salary | EmployeeIncludes.Bank | EmployeeIncludes.DependantsPersonal,


    MedicalList = EmployeeIncludes.Personal | EmployeeIncludes.Contact | EmployeeIncludes.Employment | 
      EmployeeIncludes.Medical |
      EmployeeIncludes.DependantsPersonal | DependantsMedical,


    DependantList = EmployeeIncludes.Dependants | EmployeeIncludes.DependantsPersonal,


    Minimum = EmployeeIncludes.Person | EmployeeIncludes.Employment,
    MedicalMinimum = EmployeeIncludes.Person | EmployeeIncludes.Employment | EmployeeIncludes.Medical | EmployeeIncludes.Dependants 
      | EmployeeIncludes.DependantsPerson | EmployeeIncludes.DependantsMedical,
  }

}