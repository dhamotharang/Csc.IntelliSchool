using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class ZeroBoolConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null ||  System.Convert.ToDecimal(value) == 0) {
        return false;
      }

      return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
