using System;
using System.Windows;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  /// <summary>
  /// http://stackoverflow.com/questions/3862385/wpf-validationrule-with-dependency-property
  /// </summary>
  public class DependencyValue : DependencyObject {
    public Object BindingToTrigger {
      get { return (Object)GetValue(BindingToTriggerProperty); }
      set { SetValue(BindingToTriggerProperty, value); }
    }
    public Object Value {
      get { return (Object)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty BindingToTriggerProperty =
      DependencyProperty.Register("BindingToTrigger", typeof(Object), typeof(DependencyValue),
      new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(Object), typeof(DependencyValue), 
      new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        //new PropertyMetadata(null, OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      BindingExpressionBase exprBase = BindingOperations.GetBindingExpressionBase(d, BindingToTriggerProperty);
      if (exprBase != null)
        exprBase.UpdateSource();
    }
  }
}