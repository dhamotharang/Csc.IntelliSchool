using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class PercentConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return null;

      return System.Convert.ToDecimal(value) * 100;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return null;

      return System.Convert.ToDecimal(value) / 100;
    }
  }
}
