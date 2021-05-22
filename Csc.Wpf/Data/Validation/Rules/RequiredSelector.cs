using Csc.Wpf.Properties;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class RequiredSelector : ValidationRule {

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      if (value == null || (value is int && (int)value == 0))
        return new ValidationResult(false, Resources.Validation_Required);

      return new ValidationResult(true, null);
    }
  }
}