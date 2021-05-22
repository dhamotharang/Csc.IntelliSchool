using Csc.Components.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class AsyncExtensions {
    public static void BeginInvoke(this ContentControl ctl, Action invoke) {
      ctl.Dispatcher.BeginInvoke(invoke);
    }

    public static void SafeBeginInvoke(this ContentControl ctl, Action invoke) {
      ctl.Dispatcher.BeginInvoke((Action)(()=> {
        object lockObj = null;

        if (ctl is ILock)
          lockObj = ((ILock)ctl).LockObject;
        else
          lockObj = ctl;

        lock (lockObj) {
          invoke();
        }
      }));
    }
  }
}