using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class NullConverter : IValueConverter {
    public object TargetNull { get; set; }
    public object SourceNull { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return TargetNull;

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      // TODO: Find better
      if (value == null || value.ToString().CompareTo(TargetNull.ToString()) == 0)
        return SourceNull;

      return value;
    }
  }
}
