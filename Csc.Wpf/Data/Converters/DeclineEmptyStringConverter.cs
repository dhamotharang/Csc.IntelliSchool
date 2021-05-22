using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class DeclineEmptyStringConverter : IValueConverter {
    public bool Trim { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value is string && value.ToString().Length == 0 && (Trim == false || (Trim == true && value.ToString().Trim().Length == 0)))
        return null;

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return value;
    }
  }
}
