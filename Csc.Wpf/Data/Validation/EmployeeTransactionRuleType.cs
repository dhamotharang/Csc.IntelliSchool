using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Csc.IntelliSchool.Data.Validation {
  public class EmployeeTransactionRuleType : ValidationRule {
    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      BindingGroup binding = (BindingGroup)value;
      EmployeeTransactionRule rule = (EmployeeTransactionRule)binding.Items[0];

      if (rule.Time == new TimeSpan(00, 00, 00))
        return new ValidationResult(false, "Age is not a whole number");

      return new ValidationResult(true, null);
    }
  }
}