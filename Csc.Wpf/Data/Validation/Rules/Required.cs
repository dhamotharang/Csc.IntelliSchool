using Csc.Wpf.Properties;
using System;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class Required : ValidationRule {
    public DependencyValue Minimum { get; set; }
    public DependencyValue Maximum { get; set; }
    public bool NonZero { get; set; }

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      if (value == null 
        || (value is string && value.ToString().Trim().Length == 0) 
        || (value is DateTime && ((DateTime)value) == DateTime.MinValue)
        || (Maximum != null && Convert.ToDecimal(value) > Convert.ToDecimal(Maximum.Value))
        || (Minimum != null && Convert.ToDecimal(value) < Convert.ToDecimal(Minimum.Value))
        || (NonZero == true && Convert.ToDecimal(value) == 0 ))
        return new ValidationResult(false, Resources.Validation_Required);
      
      return new ValidationResult(true, null);
    }
  }
}