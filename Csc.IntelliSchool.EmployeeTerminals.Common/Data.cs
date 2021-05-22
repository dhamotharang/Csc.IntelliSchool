using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.EmployeeTerminals.Common {
  public class Data {
    public static EmployeeTerminal GetTerminal(int id) {
      using (var ent = new DataEntities()) {
        return ent.EmployeeTerminals.SingleOrDefault(s => s.TerminalID == id);
      }
    }

    public static void SaveTerminalTransactions(string ip, IEnumerable<TerminalLogEntry> records) {
      if (records.Count() == 0)
        return;

      using (var ent = new DataEntities()) {
        foreach (var rec in records) {
          EmployeeTerminalTransaction trans = new EmployeeTerminalTransaction();
          trans.TerminalIP = ip;
          trans.DateTime = rec.DateTime ;
          trans.UserID = (int) rec.UserID;

          ent.EmployeeTerminalTransactions.Add(trans);
        }

        ent.SaveChanges();
      }
    }
  }
}
