using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;

namespace Csc.Components.Common {
  public static partial class AppExtensions {
    public static FileVersionInfo GetInfo() {
      return FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
    }

    public static string GetVersion() {
      if (ApplicationDeployment.IsNetworkDeployed)
        return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
      else
        return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
    }


    public static string GetDetailedMessage(this Exception exception, bool includeStackTrace = true) {
      string str = string.Empty;
      GetFullErrorDetails(exception, includeStackTrace, ref str);
      return str;
    }
    private static void GetFullErrorDetails(Exception exception, bool includeStackTrace, ref string errorMessage) {
      errorMessage += string.Format("{0}: {1}\n", exception.GetType().FullName, exception.Message);
      if (includeStackTrace)
        errorMessage += string.Format("{0}\n", exception.StackTrace);
      errorMessage += string.Format("{0}\n", new string('-', 30));

      if (exception.InnerException != null)
        GetFullErrorDetails(exception.InnerException, includeStackTrace, ref errorMessage);
    }

  }
}
