using System;
using System.Windows.Data;

namespace Csc.Wpf.Data {
  public class ValueSelector : IValueConverter {
    public string PropertyName { get; set; }
    public object Object { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return null;

      Object = value;

      var prop = value.GetType().GetProperty(PropertyName);
      if (prop == null)
        throw new InvalidOperationException();

      return prop.GetValue(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (value == null)
        return null;

      var prop = Object.GetType().GetProperty(PropertyName);
      if (prop == null)
        throw new InvalidOperationException();

      prop.SetValue(Object, value);

      return Object;
    }
  }
}
