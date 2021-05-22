using Csc.Components.Common;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Csc.Components.Common {
  public static partial class ProcessExtensions {
    public static void RegisterLibrary(string lib, bool silent, Action<RegisterLibraryResult> callback = null) {
      try {
        // TODO: refactor
        ProcessStartInfo info = new ProcessStartInfo("regsvr32.exe", "\"" + lib + "\"");
        if (silent) {
          info.CreateNoWindow = true;
          info.WindowStyle = ProcessWindowStyle.Hidden;
          info.Arguments = "/s " + info.Arguments;
        }
        info.Verb = "runas";

        var proc = new Process();
        proc.EnableRaisingEvents = true;
        proc.StartInfo = info;
        proc.Exited += (o, e) => {
          if (callback != null)
            Async.BeginInvoke(() => callback(new RegisterLibraryResult(lib, (RegisterLibraryStatus)proc.ExitCode)));
        };
        proc.Start();
      } catch (Exception ex) {
        if (callback != null)
          callback(new RegisterLibraryResult(lib, ex));
      }
    }

    public static void Start(string filename) {
        System.Diagnostics.Process.Start(filename);
    }
  }
}