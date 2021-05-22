using System.Globalization;
using System.Threading;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class PickerExtensions {
    public static bool IsUser(this System.Windows.Controls.SelectionChangedEventArgs e) {
      return e.Source is RadComboBox && ((RadComboBox)e.Source).IsDropDownOpen;
    }

    public static void SetPickerTypeMonth(this RadDatePicker ctl) {SetPickerType(ctl, Telerik.Windows.Controls.Calendar.DateSelectionMode.Month); }
    public static void SetPickerTypeYear(this RadDatePicker ctl) {SetPickerType(ctl, Telerik.Windows.Controls.Calendar.DateSelectionMode.Year); }
    public static void SetPickerType(this RadDatePicker ctl, Telerik.Windows.Controls.Calendar.DateSelectionMode pickerType) {
      ctl.DateSelectionMode = pickerType;

      if (pickerType == Telerik.Windows.Controls.Calendar.DateSelectionMode.Month || pickerType == Telerik.Windows.Controls.Calendar.DateSelectionMode.Year) {
        ctl.DisplayFormat = DateTimePickerFormat.Long;

        var culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID);
        if (pickerType == Telerik.Windows.Controls.Calendar.DateSelectionMode.Month)
          culture.DateTimeFormat.LongDatePattern = "MMMM, yyyy";
        else if (pickerType == Telerik.Windows.Controls.Calendar.DateSelectionMode.Year)
          culture.DateTimeFormat.LongDatePattern = "yyyy";

        ctl.Culture = culture;
      } else {
        ctl.DisplayFormat = DateTimePickerFormat.Short;
        ctl.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
      }

    }

   


    public static void FormatTimeSpan(this RadTimePicker ctl) {
      var culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID);
      culture.DateTimeFormat.ShortTimePattern = "HH:mm";

      ctl.Culture = culture;
      ctl.DisplayFormat = DateTimePickerFormat.Short;
    }
  }

  public enum PickerType {
    Day,
    Month ,
    Year
  }
}