using System;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class Performance{
    /// <summary>
    /// Disables automation mode (as recommended in  https://docs.telerik.com/devtools/wpf/common-ui-automation) and gestures.
    /// </summary>
    public static void ApplyTelerikRecommendations() {
      // As recommended by Telerik (https://docs.telerik.com/devtools/wpf/common-ui-automation) for better performance.
      // This will disable Microsoft UI Automation support – the accessibility framework for Microsoft Windows. 
      Telerik.Windows.Automation.Peers.AutomationManager.AutomationMode = Telerik.Windows.Automation.Peers.AutomationMode.Disabled;
      // Disables guestures , as recommended by Telerik for bettern perfroamance
      Telerik.Windows.Input.Touch.TouchManager.IsTouchEnabled = false;
    }

    /// <summary>
    /// Can be called inside main window ctor.
    /// </summary>
    public static void ApplyBitmapRecommendations(DependencyObject obj) {
      // As recommended by Stefan Olson: http://www.olsonsoft.com/blogs/stefanolson/error404.aspx?aspxerrorpath=/blogs/stefanolson/post/Workaround-for-low-quality-bitmap-resizing-in-WPF-4.aspx
      System.Windows.Media.RenderOptions.SetBitmapScalingMode(obj, System.Windows.Media.BitmapScalingMode.LowQuality);
    }

  }
}