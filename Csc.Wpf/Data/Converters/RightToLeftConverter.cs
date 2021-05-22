using System;
using System.Windows;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class RightToLeftConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null || (bool)value == false)
        return FlowDirection.LeftToRight;
      else 
        return FlowDirection.RightToLeft;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
