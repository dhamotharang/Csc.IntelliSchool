using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class NullValueConverter : IValueConverter {
    public object NullValue { get; set; }
    public object NonNullValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return NullValue;

      return NonNullValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
