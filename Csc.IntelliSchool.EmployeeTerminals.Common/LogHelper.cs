using NLog;
using System;
using System.IO;
using System.Linq;

namespace Csc.IntelliSchool.EmployeeTerminals.Common {
  public static class LogHelper {
    public static void OpenLogFile(string targetName) {

      try {
        var target = LogManager.Configuration.FindTargetByName(targetName) as NLog.Targets.FileTarget;
        var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
        var filename = target.FileName.Render(logEventInfo);

        filename = Path.Combine(new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory.FullName, filename);

        System.Diagnostics.Process.Start(filename);
      } catch (Exception ex) {
        LogManager.GetCurrentClassLogger().Fatal(ex);
      }
    }

    public static void InitializeLog() {
      System.Diagnostics.Trace.Listeners.OfType<System.Diagnostics.DefaultTraceListener>().First().AssertUiEnabled = false;
      GlobalDiagnosticsContext.Set("ip", Helpers.GetClientIP());
      GlobalDiagnosticsContext.Set("version", Helpers.AppVersion);
    }
  }
}
