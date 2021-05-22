
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeDeduction {
    public static EmployeeDeduction CreateObject(int employeeId) {
      var itm = new EmployeeDeduction();
      itm.EmployeeID = employeeId;
      itm.Date = DateTime.Today;

      return itm;
    }
  }
}
