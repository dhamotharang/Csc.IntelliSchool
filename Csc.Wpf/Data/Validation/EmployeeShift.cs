using System;
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Data.Validation {
  public class EmployeeShift : ValidationRule {
    public EmployeeShiftValues InputValues { get; set; }

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      if (InputValues == null || value is bool? == false || ((bool)value == true && InputValues.IsValid == false))
        return new ValidationResult(false, "Invalid");
      
      return new ValidationResult(true, null);
    }
  }

  public class EmployeeShiftValues : DependencyObject {
    public DateTime? FromTime {
      get { return (DateTime?)GetValue(FromTimeProperty); }
      set { SetValue(FromTimeProperty, value); }
    }
    public DateTime? ToTime {
      get { return (DateTime?)GetValue(ToTimeProperty); }
      set { SetValue(ToTimeProperty, value); }
    }




    public Object Object {
      get { return (Object)GetValue(ObjectProperty); }
      set { SetValue(ObjectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Object.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ObjectProperty =
        DependencyProperty.Register("Object", typeof(Object), typeof(EmployeeShiftValues), new PropertyMetadata(null));




    public static readonly DependencyProperty FromTimeProperty =
        DependencyProperty.Register("FromTime", typeof(DateTime?), typeof(EmployeeShiftValues), new PropertyMetadata(null));
    public static readonly DependencyProperty ToTimeProperty =
       DependencyProperty.Register("ToTime", typeof(DateTime?), typeof(EmployeeShiftValues), new PropertyMetadata(null));

    public bool IsValid { get { return FromTime != null && ToTime != null && FromTime < ToTime; } }
  }
}