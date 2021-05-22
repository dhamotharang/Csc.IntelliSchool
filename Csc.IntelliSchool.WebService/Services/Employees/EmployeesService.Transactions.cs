using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    public EmployeeTerminalTransaction[] InsertEmployeeTerminalLogEntries(EmployeeTerminalLogEntry[] logEntries) {
      List<EmployeeTerminalTransaction> transactions = new List<EmployeeTerminalTransaction>(logEntries.Count());

      using (var ent = ServiceManager.CreateModel()) {
        foreach (var entry in logEntries) {
          var trans = new EmployeeTerminalTransaction() {
            TerminalIP = entry.IP,
            UserID = entry.UserID,
            DateTime = entry.DateTime
          };

          transactions.Add(trans);
          ent.EmployeeTerminalTransactions.Add(trans);
        }

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Insert, typeof(EmployeeTerminalTransaction), null,
          new SystemLogDataEntry() { Count = transactions.Count() });

        ent.SaveChanges();

      }

      return transactions.ToArray();
    }

    public EmployeeTerminalTransaction[] GetEmployeeTerminalTransactions(string ip, int userId, DateTime month) {
      DateTime startDate = month.ToMonth();
      DateTime endDate = month.ToMonthEnd().ToDayEnd();

      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeTerminalTransactions.Where(s => s.TerminalIP == ip && s.UserID == userId && s.DateTime >= startDate && s.DateTime <= endDate).OrderBy(s => s.DateTime).ToArray();
      }
    }
  }

}
