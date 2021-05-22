using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningDepartmentVacationsControl : Csc.Wpf.UserControlBase, IEarningControl {
    private ObservableCollection<EmployeeDepartmentVacation> _items;

    public Employee Employee { get { return DataContext as Employee; } }
    public ObservableCollection<EmployeeDepartmentVacation> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? LastSelectedMonth { get; set; }
    public PeriodFilter? LastSelectedFilter { get; set; }
    public DateTime? PickedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } set { this.MonthDatePicker.SelectedDate = value.ToMonth(); } }
    public EmployeeEarningSection Section { get { return EmployeeEarningSection.DepartmentVacations; } }


    public EarningDepartmentVacationsControl() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
      this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
      //this.ItemsGridView.SortBy("Start");
    }

    #region Loading
    private void UserControlBase_Initialized(object sender, System.EventArgs e) { OnLoadData(); }
    private void FilterToggleButton_Click(object sender, RoutedEventArgs e) {
      if (this.FilterCurrentRadioButton.IsChecked == true)
        this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
      OnLoadData();
    }
    private void MonthDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (PickedMonth != null && PickedMonth != DateTime.Today.ToMonth())
        this.FilterSpecificRadioButton.IsChecked = true; 
      OnLoadData();
    }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { OnLoadData(); }
    public void OnLoadData(bool force = false) {
      if (Employee == null || ParentTabSelected == false || Employee.DepartmentID==null)
        return;

      PeriodFilter filter;
      DateTime month = DateTime.Today.ToMonth();

      if (this.FilterCurrentRadioButton.IsChecked == true || this.FilterSpecificRadioButton.IsChecked == true)
        filter = PeriodFilter.Current;
      else if (this.FilterUpcomingRadioButton.IsChecked == true)
        filter = PeriodFilter.Upcoming;
      else if (this.FilterPastRadioButton.IsChecked == true)
        filter = PeriodFilter.Past;
      else {
        Items = new ObservableCollection<EmployeeDepartmentVacation>();
        return;
      }

      if (this.FilterSpecificRadioButton.IsChecked == true) {
        if (this.MonthDatePicker.SelectedDate != null)
          month = this.MonthDatePicker.SelectedDate.Value.ToMonth();
        else {
          Items = new ObservableCollection<EmployeeDepartmentVacation>();
          return;
        }
      }

      if (false == force && filter == LastSelectedFilter && month == LastSelectedMonth) {
        return;
      }

      LastSelectedFilter = filter;
      LastSelectedMonth = month;

      this.SetBusy();
      EmployeesDataManager.GetDepartmentVacations(Employee.DepartmentID.Value, month, filter, OnDataLoaded);
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData(true);
    }

    private void OnDataLoaded(EmployeeDepartmentVacation[] result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        //bool sort = (Items == null || Items.Count() == 0) && this.ItemsGridView.SortDescriptors.Count() == 0;
        Items = new ObservableCollection<EmployeeDepartmentVacation>(result.OrderBy(s => s.StartDate).ToArray());
        //if (sort)
        //  this.ItemsGridView.SortBy("Start");
      } else
        Popup.AlertError(error);
    }
    #endregion


    #region ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


    }


    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData(true);
    }

    #endregion


    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }

  }
}
