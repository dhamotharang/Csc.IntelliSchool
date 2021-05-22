using System;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class WindowExtensions {
    public static ContentControl FindOwnerWindow(FrameworkElement ctl) {
      var owner = (ctl as RadWindow) as ContentControl;
      if (owner == null)
        owner = ctl.ParentOfType<RadWindow>();
      if (owner == null)
        owner = ctl.ParentOfType<Window>();
      if (owner == null)
        owner = Application.Current.MainWindow;
      return owner;
    }

    public static void DisplayModal<T>(this FrameworkElement ctl, T wnd, object state, Action<T, object> closed = null) where T: RadWindow {
      if (closed != null)
        wnd.Closed += (o, e) => closed((T)o, state);
      wnd.Owner = FindOwnerWindow(ctl);
      wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      wnd.ShowDialog();
    }

    public static void DisplayModal<T>(this FrameworkElement ctl, T wnd, Action<T> closed = null) where T : RadWindow {
      if (closed != null)
        DisplayModal<T>(ctl, wnd, null, (w, e) => closed(w));
      else
        DisplayModal<T>(ctl, wnd, null, null);
    }



    #region Display Report
    public static void DisplayReport(this FrameworkElement ctl, Telerik.Reporting.Report report, object dataSource) {
      DisplayReport(ctl, report, dataSource, null);
    }
    public static void DisplayReport(this FrameworkElement ctl, Telerik.Reporting.Report report, object dataSource, Action completed) {
      var wnd = new Views.ReportWindow(report, dataSource);
      wnd.Owner = FindOwnerWindow(ctl);
      if (completed != null)
        wnd.Closed += (o, e) => { completed(); };
      wnd.ShowDialog();
    }
    public static void DisplayFile(this FrameworkElement ctl, string title, string filename, Action completed = null) {
      var wnd = new Views.WebBrowserWindow(title, filename);
      wnd.Owner = FindOwnerWindow(ctl);
      if (completed != null)
        wnd.Closed += (o, e) => { completed(); };
      wnd.ShowDialog();
    }
    #endregion

  }
}