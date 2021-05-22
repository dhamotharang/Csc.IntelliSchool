using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeSalaryUpdatesControl : Csc.Wpf.UserControlBase {
    private ObservableCollection<EmployeeSalaryUpdate> _items;

    public Employee Employee { get { return DataContext as Employee; } }
    public ObservableCollection<EmployeeSalaryUpdate> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? LastSelectedYear{ get; set; }
    public PeriodFilter? LastSelectedFilter { get; set; }

    public EmployeeSalaryUpdatesControl() {
      InitializeComponent();
      this.YearDatePicker.SetPickerTypeYear();
      this.YearDatePicker.SelectedDate = DateTime.Today.ToYear();
      this.ItemsGridView.SortByDescending("Date");
    }

    private void UserControlBase_Initialized(object sender, System.EventArgs e) { LoadData(); }
    private void FilterToggleButton_Click(object sender, RoutedEventArgs e) { LoadData(); }
    private void YearDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) { LoadData(); }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { LoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }

    private void OnLoadData() {
      LoadData(true);
    }

    public void LoadData(bool force = false) {
      if (Employee == null || this.ParentTabSelected == false)
        return;

      PeriodFilter filter;
      DateTime year = DateTime.Today.ToYear();

      if (this.FilterCurrentRadioButton.IsChecked == true || this.FilterSpecificRadioButton.IsChecked == true)
        filter = PeriodFilter.Current;
      else if (this.FilterPastRadioButton.IsChecked == true)
        filter = PeriodFilter.Past;
      else if (this.FilterAllRadioButton.IsChecked == true)
        filter = PeriodFilter.All;
      else {
        Items = new ObservableCollection<EmployeeSalaryUpdate>();
        return;
      }

      if (this.FilterSpecificRadioButton.IsChecked == true) {
        if (this.YearDatePicker.SelectedDate != null)
          year = this.YearDatePicker.SelectedDate.Value.ToYear();
        else {
          Items = new ObservableCollection<EmployeeSalaryUpdate>();
          return;
        }
      }

      if (force == false && filter == LastSelectedFilter && year == LastSelectedYear) {
        return;
      }

      LastSelectedFilter = filter;
      LastSelectedYear = year;

      this.SetBusy();
      EmployeesDataManager.GetEmployeeSalaryUpdates(Employee.EmployeeID, year.Year, filter, OnDataLoaded);
    }

    private void OnDataLoaded(EmployeeSalaryUpdate[] result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        //bool sort = (Items == null || Items.Count() == 0) && this.ItemsGridView.SortDescriptors.Count() == 0;
        Items = new ObservableCollection<EmployeeSalaryUpdate>(result.OrderBy(s => s.Date).ToArray());
        //if (sort)
        //  this.ItemsGridView.SortBy("Date");
      } else
        Popup.AlertError(error);
    }


    #region Context Menu

    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData();
    }


    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }
    #endregion
  }
}
