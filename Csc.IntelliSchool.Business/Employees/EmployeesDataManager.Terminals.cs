using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Terminals
    public static void AddOrUpdateTerminal(EmployeeTerminal item, AsyncState<EmployeeTerminal> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateTerminal(item), callback);
    }

    public static void DeleteTerminal(EmployeeTerminal item, AsyncState<EmployeeTerminal> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteTerminal(item.TerminalID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }


    public static void GetTerminals(AsyncState<EmployeeTerminal[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetTerminals(), callback);
    }

    public static void FetchTerminalTransactions(EmployeeTerminal terminal, bool clearAfterCompletion, AsyncState<EmployeeTerminalFetchResult> callback) {
      Async.AsyncCall(() => FetchEmployeeTerminalTransactions(terminal, clearAfterCompletion), callback);
    }
    #endregion



    private static EmployeeTerminalFetchResult FetchEmployeeTerminalTransactions(EmployeeTerminal terminal, bool clearAfterCompletion) {
      EmployeeTerminalFetchResult res = new EmployeeTerminalFetchResult();
      List<Exception> errors = new List<Exception>();

      try {
        using (var ping = new Ping()) {
          if (ping.Send(terminal.IP).Status != IPStatus.Success)
            throw new UnauthorizedAccessException("Device inaccessible.");
        }

        res.IP = terminal.IP;

        EmployeeTerminalDevice dvc = null;

        if (terminal.TerminalModel == EmployeeTerminalModel.Realand) {
          dvc = new EmployeeTerminalDeviceReland(terminal.IP, terminal.Port.Value, terminal.MachineID.Value, terminal.Password.Value);
        } else {
          throw new PlatformNotSupportedException();
        }

        dvc.Initialize();

        dvc.Connect();

        var logEntries = dvc.ReadLogEntries().ToArray();
        Service.EmployeesService.Instance.InsertEmployeeTerminalLogEntries(logEntries);

        res.EntryCount = logEntries.Count();
        res.UserCount = logEntries.Select(s => s.UserID).Distinct().Count();

        if (clearAfterCompletion) {
          try {
            dvc.ClearLogEntries();
          } catch (Exception ex) {
            errors.Add(ex);
          }
        }

        try {
          dvc.Disconnect();
        } catch (Exception ex) {
          errors.Add(ex);
        }
      } catch (Exception ex) {
        errors.Add(ex);
      }

      if (errors.Count() > 0)
        res.Error = new AggregateException(errors.ToArray());
      return res;
    }

  }
}