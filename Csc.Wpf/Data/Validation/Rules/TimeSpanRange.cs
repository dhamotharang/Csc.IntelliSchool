using Csc.Wpf.Properties;
using System;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class TimeSpanRange : ValidationRule {
    public TimeSpan? Minimum { get; set; }
    public TimeSpan? Maximum { get; set; }
    public bool IsRequired { get; set; }

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      if ((IsRequired && value == null)
        || (value != null && Minimum != null && Minimum.Value > (TimeSpan)value)
        || (value != null && Maximum != null && Maximum.Value < (TimeSpan)value))
        return new ValidationResult(false, Resources.Validation_Required);

      return new ValidationResult(true, null );
    }
  }
}