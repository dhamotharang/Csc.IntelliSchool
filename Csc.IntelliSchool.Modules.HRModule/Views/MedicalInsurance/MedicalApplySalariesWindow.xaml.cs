using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalApplySalariesWindow : Csc.Wpf.WindowBase {
    private RadGridView _gridView;

    public RadGridView GridView { get { return _gridView; } set { _gridView = value; OnPropertyChanged(() => GridView); } }

    public Employee[] Items { get { return this.FilterList.Items != null ? this.FilterList.Items.Select(s => (Employee)s).ToArray() : null; } }

    // Constructors
    public MedicalApplySalariesWindow() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }

    private void ApplyButton_Click(object sender, RoutedEventArgs e) {
      if (Items == null) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoSelectedItems);
        return;
      }

      if (this.EarningsCheckBox.IsChecked == true && this.MonthDatePicker.SelectedDate == null) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoSelectedMonth);
        return;
      }

      if (this.EarningsCheckBox.IsChecked == false && this.SalariesCheckBox.IsChecked == false) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoOptionSelected);
        return;
      }

      if (Items.Count() == 0) {
        this.Close(OperationResult.None);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.ApplyMedicalSalaries(
        Items.Select(s => s.EmployeeID).ToArray(),
        this.SalariesCheckBox.IsChecked == true,
        this.EarningsCheckBox.IsChecked == true ? this.MonthDatePicker.SelectedDate.Value.ToMonth() : new DateTime?(),
        OnApplyCompleted);
    }

    private void OnApplyCompleted(Employee[] result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        foreach (var emp in Items)
          emp.Salary.Medical = emp.MedicalInfo.ActiveMonthlyTotal ?? 0;
        this.Close(true);
      } else {
        Popup.AlertError(error);
      }
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {

    }
  }
}