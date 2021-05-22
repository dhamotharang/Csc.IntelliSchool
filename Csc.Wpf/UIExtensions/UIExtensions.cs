using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class UIExtensions {
    public static void Rebind(this IEnumerable<FrameworkElement> elements, DependencyProperty dp = null) {
      foreach (var elem in elements)
        Rebind(elem, dp);
    }
    /// <summary>
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="dp">When null, rebinds default property.</param>
    public static void Rebind(this FrameworkElement elem, DependencyProperty dp = null) {
      if (dp == null) {
        if (elem is TextBlock)
          dp = TextBlock.TextProperty;
        else if (elem is Image)
          dp = Image.SourceProperty;
        else if (elem is RadNumericUpDown)
          dp = RadNumericUpDown.ValueProperty;
        else if (elem is TextBox)
          dp = TextBox.TextProperty;
        else if (elem is RadWatermarkTextBox)
          dp = RadWatermarkTextBox.TextProperty;
        else if (elem is RadDateTimePicker)
          dp = RadDateTimePicker.SelectedValueProperty;
        else if (elem is RadToggleButton)
          dp = RadToggleButton.IsCheckedProperty;
        else if (elem is RadRadioButton)
          dp = RadRadioButton.IsCheckedProperty;
        else if (elem is RadComboBox)
          dp = RadComboBox.SelectedValueProperty;
        else if (elem is CheckBox)
          dp = CheckBox.IsCheckedProperty;
      }

      elem.GetBindingExpression(dp).UpdateTarget();
    }


    public static void SetNullIfEmpty(this RadDatePicker elem) {
      if (elem.SelectedValue == DateTime.MinValue)
        elem.SelectedValue = null;
    }

    public static T FindVisualParent<T>(this DependencyObject child) where T : class {
      // get parent item
      DependencyObject parentObject = VisualTreeHelper.GetParent(child);
      
      // we’ve reached the end of the tree
      if (parentObject == null) return null;

      // check if the parent matches the type we’re looking for
      T parent = parentObject as T;
      if (parent != null) {
        return parent;
      } else {
        // use recursion to proceed with next level
        return FindVisualParent<T>(parentObject);
      }
    }

    public static T FindVisualChild<T>(this DependencyObject parent) where T : class {
      if (parent == null)
        return null;

      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
        var child = VisualTreeHelper.GetChild(parent, i);

        var result = (child as T) ?? FindVisualChild<T>(child);
        if (result != null)
          return result;
      }

      return null;
    }


    public static T FindLogicalParent<T>(this DependencyObject child) where T : class {
      // get parent item
      DependencyObject parentObject = LogicalTreeHelper.GetParent(child);

      // we’ve reached the end of the tree
      if (parentObject == null) return null;

      // check if the parent matches the type we’re looking for
      T parent = parentObject as T;
      if (parent != null) {
        return parent;
      } else {
        // use recursion to proceed with next level
        return FindLogicalParent<T>(parentObject);
      }
    }


    public static T FindLogicalChild<T>(this DependencyObject parent) where T : class {
      if (parent == null)
        return null;


      foreach (var child in LogicalTreeHelper.GetChildren(parent)) {
        var result = (child as T) ?? FindLogicalChild<T>(child as DependencyObject);
        if (result != null)
          return result;
      }

      return null;
    }



    public static T SourceOfType<T>(this RoutedEventArgs e) where T : DependencyObject {
      FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
      if (originalSender != null) {
        if (originalSender as T != null)
          return originalSender as T;
        return originalSender.ParentOfType<T>();
      } else
        return null;
    }
  }
}