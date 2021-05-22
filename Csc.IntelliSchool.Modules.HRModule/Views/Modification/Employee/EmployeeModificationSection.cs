using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  [Flags]
  public enum EmployeeModificationSection {
    None = 0,
    Personal = 1 << 0,
    Education = 1 << 1,
    Contact = 1 << 2,
    Employment = 1 << 3,
    OfficialDocuments = 1 << 4,
    Salary = 1 << 5,
    Termination = 1 << 6,
    Bank = 1<<7,

    Basic = Personal | Contact | OfficialDocuments,

    Everything = Personal | Contact | Employment | OfficialDocuments | Salary | Termination | Bank,
  }
}
