using Csc.Wpf.Properties;
using System;
using System.Windows.Controls;

namespace Csc.Wpf.Data.Validation {
  public class DateRange : ValidationRule {
    private string _errorMessage = Resources.Validation_DateRange;

    public DependencyValue StartDate { get; set; }
    public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; } }

    public bool Required { get; set; }

    public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
      DateTime? date = value as DateTime?;
      if (Required && (date == null || StartDate == null || StartDate.Value == null))
        return new ValidationResult(false, Resources.Validation_Required);

      if (date != null && StartDate.Value != null && date < (DateTime)StartDate.Value)
        return new ValidationResult(false, ErrorMessage);

      return new ValidationResult(true, null);
    }
  }

}