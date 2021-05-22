using Csc.IntelliSchool.EmployeeTerminals.Common;
using NLog;
using System;
using System.Linq;

namespace Csc.IntelliSchool.EmployeeTerminals.ZDC2911 {
  class Program {
    private static ShellArguments _shellArgs;
    private static EmployeeTerminal _terminal;


    static void Main(string[] args) {
#if DEBUG
      args = new string[] { "5", "true", "true", "true" };
#endif


      LogHelper.InitializeLog();
      try {
        LogManager.GetCurrentClassLogger().Info("Start");
        LogManager.GetCurrentClassLogger().Trace("Input Arguments: " + string.Join(", ", args));

        _shellArgs = new ShellArguments(args);

        LogManager.GetCurrentClassLogger().Trace("Current Arguments: " + _shellArgs);

        if (false == _shellArgs.LoadLog && false == _shellArgs.ClearLog)
          return;

        LogManager.GetCurrentClassLogger().Info("Retrieving terminal information");
        _terminal = Data.GetTerminal(_shellArgs.TerminalID);

        if (_terminal == null) {
          LogManager.GetCurrentClassLogger().Error("Terminal not found");
          return;
        } else {
          LogManager.GetCurrentClassLogger().Info("Terminal {0} found", _terminal.Name);
        }

        LoadLog();
        ClearLog();


      } catch (Exception ex) {
        LogManager.GetCurrentClassLogger().Fatal(ex);
      } finally {
        if (_shellArgs.Attended == true) {
          LogManager.GetCurrentClassLogger().Info("Opening log file");
          LogHelper.OpenLogFile("lastRun");
        }

        LogManager.GetCurrentClassLogger().Info("End");
      }
    }

    private static void LoadLog() {
      if (_shellArgs.LoadLog == false)
        return;

      LogManager.GetCurrentClassLogger().Info("Started loading logs");

      var logRecords = DeviceProxy.GetLog(_terminal.IP, _terminal.Port.Value, _terminal.MachineID.Value, _terminal.Password.Value.ToString());

      LogManager.GetCurrentClassLogger().Info("Found {0} records", logRecords.Count());

      LogManager.GetCurrentClassLogger().Info("Saving to database");
      Data.SaveTerminalTransactions(_terminal.IP, logRecords);

      LogManager.GetCurrentClassLogger().Info("Finished loading logs");
    }


    private static void ClearLog() {
      if (_shellArgs.ClearLog == false)
        return;

      LogManager.GetCurrentClassLogger().Info("Started cleaning logs");

      DeviceProxy.ClearLog(_terminal.IP, _terminal.Port.Value, _terminal.MachineID.Value, _terminal.Password.Value.ToString());
      LogManager.GetCurrentClassLogger().Info("Finished cleaning logs");
    }



  }
}
