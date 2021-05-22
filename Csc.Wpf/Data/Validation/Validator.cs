using Csc.Wpf.Data.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Telerik.Windows.Controls;

namespace Csc.Wpf.Data {
  public static class Validator {
    public static bool Validate(this DependencyObject parent, bool focusFirstObject = false) {
      List<DependencyObject> errors=  new List<DependencyObject>();
      var valid = Validate(parent, ref errors);
      if (focusFirstObject && errors.Count() > 0) {
        UIElement elem = errors[0] as UIElement;
        if (elem != null && elem.Focusable) {
          elem.Focus();
          var tab = elem.ParentOfType<RadTabItem>() ;
          if (tab != null)
            tab.IsSelected = true;
        }
      }
      return valid;
    }
    private static bool Validate(this DependencyObject parent, ref List<DependencyObject> errors) {
      bool valid = true;
      if ((parent is UIElement && ((UIElement)parent).Visibility == Visibility.Collapsed)
        || (parent is UIElement && ((UIElement)parent).IsEnabled==false)
        || (parent is IValidation && ((IValidation)parent).CanValidate == false))
        return true;


      LocalValueEnumerator localValues = parent.GetLocalValueEnumerator();
      while (localValues.MoveNext()) {
        LocalValueEntry entry = localValues.Current;
        if (BindingOperations.IsDataBound(parent, entry.Property)) {
          Binding binding = BindingOperations.GetBinding(parent, entry.Property);
          if (binding != null && (binding.Mode == BindingMode.Default || binding.Mode == BindingMode.OneWayToSource || binding.Mode == BindingMode.TwoWay)) {
            BindingExpression expression = BindingOperations.GetBindingExpression(parent, entry.Property);
            expression.UpdateSource();//ADDITION

            foreach (ValidationRule rule in binding.ValidationRules) {
              ValidationResult result = rule.Validate(parent.GetValue(entry.Property), null);
              if (!result.IsValid) {
                System.Windows.Controls.Validation.ClearInvalid(expression); // HACK: because in Tabs doesn't show automatically
                System.Windows.Controls.Validation.MarkInvalid(expression, new ValidationError(rule, expression, result.ErrorContent, null));
                errors.Add(parent);
                valid = false;
              } else
                System.Windows.Controls.Validation.ClearInvalid(expression);
            }
          } else {
            try {
              if (parent is FrameworkElement)
                Trace.WriteLine(string.Format("CUSTOM VALIDATOR {0} {1}", (parent as FrameworkElement).Name, entry.Property.Name));
            } catch {
            }
          }
        }
        if (parent is FrameworkElement) {
          var elem = (FrameworkElement)parent;
          if (null != elem.BindingGroup)
            elem.BindingGroup.CommitEdit();
        }
      }

      // Validate all the bindings on the children

      foreach(var child in LogicalTreeHelper.GetChildren(parent) ) {
        if (child is DependencyObject == false)
          continue;

        if (!Validate((DependencyObject)child, ref errors))
          valid = false;
      }

      // FIRST, WAY TOO SLOW
      //foreach (var child in parent.ChildrenOfType<DependencyObject>()) {
      //  if (!Validate(child, ref errors))
      //    valid = false;
      //}

      // SECOND
      //for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i) {
      //  DependencyObject child = VisualTreeHelper.GetChild(parent, i);
      //  if (!Validate(child, ref errors))
      //    valid = false;
      //}

      return valid;
    }

  }
}
