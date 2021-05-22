using Csc.Components.Common;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static class BusyHandler {
    public static void SetBusy(this FrameworkElement owner, Action clearTrigger = null) {
      var busy = FindBusyParent(owner);
      if (busy == null) 
        return;

      busy.SetBusy(true);


      if (clearTrigger != null) {
        EventHandler triggerDelegate = null;
        triggerDelegate = (EventHandler)((o, e) => {
          if (clearTrigger != null && triggerDelegate != null) {
            busy.BusyChanged -= triggerDelegate;
          }
        });

        busy.BusyChanged += triggerDelegate;
      }
    }

    public static void ClearBusy(this FrameworkElement owner) {
      var busy = FindBusyParent(owner);
      if (busy != null)
        busy.SetBusy(false);
    }

    private static IBusy FindBusyParent(FrameworkElement owner) {
      if (owner is IBusy)
        return ((IBusy)owner);

      IBusy busy = owner.FindVisualParent<IBusy>();
      if (busy == null)
        busy = owner.FindLogicalParent<IBusy>();

      return busy;
    }
  }
}