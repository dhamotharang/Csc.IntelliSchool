using Csc.Wpf.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Csc.Wpf {
  public static partial class SelectorExtensions {
    public static int[] GetSelectedItemIDs<T>(this MultiSelector ctl, Func<T, int> idCallback) where T : class {
      return ctl.SelectedItems.Cast<T>().Select(idCallback).ToArray();
    }
    public static void SetSelectedItemIDs<T>(this RadComboBox ctl, int[] selectedItemIds, Func<T, int> idCallback) where T : class {
      if (ctl.AllowMultipleSelection)
        ctl.SelectedItems.Clear();
      else
        ctl.SelectedItem = null;


      if (selectedItemIds == null || selectedItemIds.Length == 0)
        return;


      Action<T> setSelectedItem = (itm) => {
        if (ctl.AllowMultipleSelection)
          ctl.SelectedItems.Add(itm);
        else
          ctl.SelectedItem = itm;
      };

      bool singleItem = selectedItemIds.Contains(0) ;

      foreach (var itm in ctl.Items.Cast<T>()) {
        var itmId = idCallback(itm);

        if (singleItem && itmId == 0) {
          setSelectedItem(itm);
          return;
        } else if (singleItem == false && selectedItemIds.Contains(itmId))
          setSelectedItem(itm);
      }
    }


    public static int? SelectedID(this Selector sel) {
      if (sel.SelectedValue == null)
        return null;

      return (int)sel.SelectedValue;
    }
  }
}