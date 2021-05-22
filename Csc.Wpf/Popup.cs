using Csc.Wpf.Properties;
using Csc.Wpf.Views;
using System;
using Csc.Components.Common;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class Popup {
    #region Params
    private static DialogParameters CreateParam(ContentControl owner, string msg, Action<DialogParameters> init, Action<bool?> closed) {
      DialogParameters param = new DialogParameters();
      param.Content = new TextBlock() { Text = msg, TextWrapping = System.Windows.TextWrapping.Wrap, Width = 400 };
      param.Closed += (o, e) => {
        if (closed != null)
          closed(e.DialogResult);
      };
      try {
        param.Header = Application.Current.MainWindow.Title;
      } catch (ArgumentNullException) {
      }
      param.DialogStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
      param.Owner = WindowExtensions.FindOwnerWindow(owner);

      if (init != null)
        init(param);
      return param;
    }
    private static void InternalAlert(ContentControl owner, string msg, Action<DialogParameters> init, Action<bool?> closed) {
      DialogParameters param = CreateParam(owner, msg, init, closed);
      RadWindow.Alert(param);
    }
    #endregion


    public static void Alert(string msg, Action<bool?> closed = null) { Alert(null, msg, closed); }
    public static void Alert(this ContentControl owner, string msg, Action<bool?> closed = null) { InternalAlert(owner, msg, null, closed); }


    public static void ConfirmDelete(Action confirmed = null) { ConfirmDelete(null, confirmed); }
    public static void ConfirmDelete(this ContentControl owner, Action confirmed = null) {
      Confirm(owner, Resources.Popup_Confirm_Delete, (res) => {
        if (res == true && confirmed != null)
          confirmed();
      });
    }
    public static void Confirm(this ContentControl owner, string msg, Action confirmed = null) {
      Confirm(owner, msg, (res) => {
        if (res == true && confirmed != null)
          confirmed();
      });
    }
    public static void Confirm(this ContentControl owner, string msg, Action<bool?> closed = null) {
      DialogParameters param = CreateParam(owner, msg, null, closed);
      RadWindow.Confirm(param);
    }

    public static void AlertError(string msg, Action<bool?> closed = null) { AlertError(null, msg, "", closed); }
    public static void AlertError(Exception ex, Action<bool?> closed = null) { AlertError(null, ex, closed); }
    public static void AlertError(this ContentControl owner, string msg, Action<bool?> closed = null) { AlertError(owner, msg, "", closed); }
    public static void AlertError(this ContentControl owner, Exception ex, Action<bool?> closed = null) { AlertError(owner, null, ex, closed); }
    public static void AlertError(this ContentControl owner, string msg, Exception error, Action<bool?> closed = null) {
      msg = (false == string.IsNullOrWhiteSpace(msg)) ? "\n" + msg : string.Empty;

      if (error is AggregateException == false)
        msg += error.GetDetailedMessage(false );
      else {
        foreach (var ex in ((AggregateException)error).InnerExceptions) {
          msg += error.GetDetailedMessage(false) + Environment.NewLine;
        }
      }
      AlertError(owner, null, msg, closed);
    }
    public static void AlertError(this ContentControl owner, string msg, string details, Action<bool?> closed = null) {
      if (owner == null)
        owner = Application.Current.MainWindow;
      if (string.IsNullOrEmpty(msg))
        msg = Resources.Popup_Error_Unknown; // TODO: find better way

      ErrorWindow wnd = new ErrorWindow();
      wnd.Header = Application.Current.MainWindow.Title;
      wnd.Owner = owner;
      wnd.Message = msg;
      wnd.Details = details;

      wnd.Closed += (o, e) => { if (closed != null)closed(e.DialogResult); };
      wnd.ShowDialog();
    }
  }
}