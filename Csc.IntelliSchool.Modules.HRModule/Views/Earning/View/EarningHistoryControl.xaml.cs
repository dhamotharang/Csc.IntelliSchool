using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningHistoryControl : Csc.Wpf.UserControlBase, IEarningControl {
    private ObservableCollection<SingleEmployeeEarningSummary> _items;

    public Employee Employee { get { return DataContext as Employee; } }
    public ObservableCollection<SingleEmployeeEarningSummary> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? LastSelectedYear{ get; set; }
    public PeriodFilter? LastSelectedFilter { get; set; }
    public EmployeeEarningSection Section { get { return EmployeeEarningSection.History; } }

    public DateTime? PickedMonth {      get {return null; } set { }}

    public EarningHistoryControl() {
      InitializeComponent();
      this.YearDatePicker.SetPickerTypeYear();
      this.YearDatePicker.SelectedDate = DateTime.Today.ToYear();
      this.ItemsGridView.SortByDescending("Date", "Month");
    }

    private void UserControlBase_Initialized(object sender, System.EventArgs e) { OnLoadData(); }
    private void FilterToggleButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void YearDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) { OnLoadData(); }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    public void OnLoadData(bool force = false) {
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
        Items = new ObservableCollection<SingleEmployeeEarningSummary>();
        return;
      }

      if (this.FilterSpecificRadioButton.IsChecked == true) {
        if (this.YearDatePicker.SelectedDate != null)
          year = this.YearDatePicker.SelectedDate.Value.ToYear();
        else {
          Items = new ObservableCollection<SingleEmployeeEarningSummary>();
          return;
        }
      }

      if (force == false && filter == LastSelectedFilter && year == LastSelectedYear) {
        return;
      }

      LastSelectedFilter = filter;
      LastSelectedYear = year;

      this.SetBusy();
      EmployeesDataManager.GetSingleEarningsSummary(Employee.EmployeeID, year.Year, filter, OnDataLoaded);
    }

    private void OnDataLoaded(SingleEmployeeEarningSummary[] result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        bool sort = (Items == null || Items.Count() == 0) && this.ItemsGridView.SortDescriptors.Count() == 0;
        Items = new ObservableCollection<SingleEmployeeEarningSummary>(result.OrderByDescending(s => s.Month).ToArray());
        if (sort)
      this.ItemsGridView.SortByDescending("Date", "Month");
      } else
        Popup.AlertError(error);
    }

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
