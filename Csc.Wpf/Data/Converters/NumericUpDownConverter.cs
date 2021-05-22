using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class NumericUpDownConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return new double?() ;
      
      return System.Convert.ToDouble(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return value;
    }
  }
}
