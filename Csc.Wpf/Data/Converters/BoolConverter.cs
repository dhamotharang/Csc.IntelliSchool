using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class BoolConverter : IValueConverter {
    public object TrueValue { get; set; }
    public object FalseValue { get; set; }
    public object NullValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return NullValue;
      else if ((bool)value == true)
        return TrueValue;
      else if ((bool)value == false)
        return FalseValue;

      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
