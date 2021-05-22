using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class TimeSpanDateTimeConverter : IValueConverter {

    public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return null;

      TimeSpan ts = (TimeSpan)value;
      return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, ts.Hours, ts.Minutes, ts.Seconds);
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return null;

      DateTime dt = (DateTime)value;
      return new TimeSpan(dt.Hour, dt.Minute, dt.Second);
    }
  }
}
