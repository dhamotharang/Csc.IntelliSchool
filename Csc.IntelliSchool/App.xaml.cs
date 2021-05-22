using Csc.Wpf;
using Csc.Components.Common;
using System.Globalization;
using System.Windows;
using Telerik.Windows.Controls;
using System;
using System.Diagnostics;

namespace Csc.IntelliSchool {
  public partial class App : Application {
    #region Fields
    private static CultureInfo _culture;
    #endregion

    #region Properties
    public static CultureInfo CurrentCulture {
      get {
        if (null == _culture)
          lock (typeof(App))
            if (null == _culture) {
              _culture = new CultureInfo(IntelliSchool.Properties.Settings.Default.DefaultCulture);
            }
        return _culture;
      }
    }

    public static string Version {
      get {
        return AppExtensions.GetVersion();
      }
    }

    public static string Title {
      get {
        return "CSC IntelliSchool " + Version;
      }
    }
    #endregion

    public App() {
      Csc.Wpf.ApplicationExtension.ApplyCulture(this, CurrentCulture);
      this.InitializeComponent();
      this.DispatcherUnhandledException += App_DispatcherUnhandledException;

      Csc.Wpf.Performance.ApplyTelerikRecommendations();
      ApplyTheme();
    }
  

    private static void ApplyTheme() {
      StyleManager.ApplicationTheme = new Windows8Theme();
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
      e.Handled = true;

      if (HandleConfigException(e.Exception)) {
        RestartApplication();
        return;
      }
      ReportError(e.Exception);
      Popup.AlertError(e.Exception);
    }

    public static void ReportError(Exception ex) {
      EventLog.WriteEntry(Title, ex.GetDetailedMessage(), EventLogEntryType.Error);
    }

    private static bool HandleConfigException(Exception ex) {
      var configEx = ex.InnerException as System.Configuration.ConfigurationErrorsException;
      if (configEx != null) {
        var config = configEx.Filename;
        System.IO.File.Delete(config);
        return true;
      }

      return false;
    }
    private static void RestartApplication() {
      System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
      Application.Current.Shutdown();
    }
  }
}
