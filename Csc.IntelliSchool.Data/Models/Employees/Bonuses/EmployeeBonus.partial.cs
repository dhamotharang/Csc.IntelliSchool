
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeBonus {
    public static EmployeeBonus CreateObject(int employeeId) {
      var itm = new EmployeeBonus();
      itm.EmployeeID = employeeId;
      itm.IncludeInSalary = true;
      itm.Date = DateTime.Today;

      return itm;
    }
  }
}
