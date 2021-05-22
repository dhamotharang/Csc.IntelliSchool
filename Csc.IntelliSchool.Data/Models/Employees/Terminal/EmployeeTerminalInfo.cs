
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class EmployeeTerminalInfo {
    public string TerminalIP { get; set; }
    public int UserID { get; set; }

    public EmployeeTerminalInfo() {
    }

    public static EmployeeTerminalInfo FromEmployee(Employee emp) {
      if (emp.IsTerminalUser == false)
        throw new InvalidOperationException();
      return new EmployeeTerminalInfo() {
        TerminalIP = emp.Terminal.IP,
        UserID = emp.TerminalUserID.Value
      };
    }
  }


}