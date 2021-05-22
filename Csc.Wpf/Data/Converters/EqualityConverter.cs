using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class EqualityConverter : IValueConverter {
    public object TrueValue { get; set; }
    public object FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == parameter)
        return TrueValue;
      return FalseValue; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
