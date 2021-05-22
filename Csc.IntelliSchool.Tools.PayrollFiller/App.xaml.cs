using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Tools.PayrollFiller {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

      this.DispatcherUnhandledException += App_DispatcherUnhandledException;

      Telerik.Windows.Controls.StyleManager.ApplicationTheme = new VisualStudio2013Theme();
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
      e.Handled = true;
      MessageBox.Show(Application.Current.MainWindow, e.Exception.Message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
}
