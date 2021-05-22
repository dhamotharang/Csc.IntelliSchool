using Csc.Wpf.Properties;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class Numbers : ValidationRule {
    public bool Required { get; set; }

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      if (Required && (value == null || value.ToString().Trim().Length == 0 ))
        return new ValidationResult(false, Resources.Validation_Required);
      foreach (var c in value.ToString()) {
        if(char.IsDigit(c) == false)
          return new ValidationResult(false, Resources.Validation_Invalid);
      }

      return new ValidationResult(true, null);
    }
  }
}