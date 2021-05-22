using Csc.Wpf.Properties;
using System.Net;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class IP : ValidationRule {
    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {

      if (value == null || value is string == false)
        return new ValidationResult(false, Resources.Validation_Invalid);

      IPAddress ip = null;
      if (IPAddress.TryParse(value.ToString(), out ip))
        return new ValidationResult(true, null);

      return new ValidationResult(false, Resources.Validation_Invalid);
    }
  }
}