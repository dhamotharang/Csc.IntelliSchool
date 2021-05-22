using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  [Flags]
  public enum EmployeeEarningSection {
    None = 0,
    Attendance = 1 << 1,
    Allowances = 1 << 2,
    Charges = 1 << 3,
    Bonuses = 1 << 4,
    Deductions = 1 << 5,
    Vacations = 1 << 6,
    DepartmentVacations = 1 << 7,
    Loans = 1 << 8,
    History = 1 << 9,
    Summary = 1 << 10,

    All = Attendance | Allowances | Charges | Bonuses | Deductions | Vacations | DepartmentVacations | Loans | History | Summary
  }

}
