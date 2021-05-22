using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  public class EmployeeTerminalInfoComparer : IEqualityComparer<EmployeeTerminalInfo> {

    public bool Equals(EmployeeTerminalInfo x, EmployeeTerminalInfo y) {
      return x == y || (x.TerminalIP == y.TerminalIP && x.UserID == y.UserID);
    }

    public int GetHashCode(EmployeeTerminalInfo obj) {
      return obj.GetHashCode();
    }
  }


}