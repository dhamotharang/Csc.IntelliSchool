using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;
using System.Windows.Markup;

namespace Csc.Wpf {
  public static partial class ApplicationExtension {
    public static void ApplyCulture(this Application app, System.Globalization.CultureInfo targetCulture) {
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
        new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(targetCulture.IetfLanguageTag)));

      System.Globalization.CultureInfo.DefaultThreadCurrentCulture = targetCulture;
      System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = targetCulture;
      System.Threading.Thread.CurrentThread.CurrentCulture = targetCulture;
      System.Threading.Thread.CurrentThread.CurrentUICulture = targetCulture;
    }




    public static void CloseAllWindows(this Application app, bool includeMainWindow = true) {
      for (int i = app.Windows.Count - 1; i >= 0; i--) {
        Window wnd = app.Windows[i];
        if (includeMainWindow == false && wnd == app.MainWindow)
          continue;

        wnd.Closing += (o, e) => {
          e.Cancel = false;
        };
        wnd.Close();
      }
    }
  }
}