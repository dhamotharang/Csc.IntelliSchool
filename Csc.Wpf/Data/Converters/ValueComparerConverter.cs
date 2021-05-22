using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class ValueComparerConverter : IValueConverter {
    public object TrueValue { get; set; }
    public object FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if ((value == null && parameter == null) || (value != null && value.Equals(parameter)))
        return TrueValue;
      else
        return FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
