using System;
using Csc.Components.Common;
using System.Windows;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class FlowDirectionConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return FlowDirection.LeftToRight;
      else {
        return ((string)value).StartsWithArabicLetter() ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
