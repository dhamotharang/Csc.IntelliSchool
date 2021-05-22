using Csc.Wpf.Properties;
using System;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class RequiredNumber : ValidationRule {
    public bool NonZero { get; set; }
    public DependencyValue Maximum { get; set; }

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      if (value == null 
        || value.ToString().Trim().Length == 0 
        || (NonZero && Convert.ToDecimal(value) == 0)
        || (Maximum != null && Convert.ToDecimal(value) > Convert.ToDecimal(Maximum.Value)))
        return new ValidationResult(false, Resources.Validation_Required);

      return new ValidationResult(true, null);
    }
  }
}