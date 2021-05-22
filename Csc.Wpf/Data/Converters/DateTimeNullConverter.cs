using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class DateTimeNullConverter : IValueConverter {
    public bool Required { get; set; }
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null || ((DateTime)value ) == DateTime.MinValue || ((DateTime)value) == DateTime.MaxValue)
        return null;

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (Required && value == null)
        return DateTime.MinValue;

      return value;
    }
  }
}
