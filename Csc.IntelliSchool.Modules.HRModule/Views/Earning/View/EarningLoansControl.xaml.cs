using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningLoansControl : Csc.Wpf.UserControlBase, IEarningControl {
    private bool _hasUpdates;
    private ObservableCollection<EmployeeLoan> _items;

    public Employee Employee { get { return DataContext as Employee; } }
    public ObservableCollection<EmployeeLoan> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? LastSelectedMonth { get; set; }
    public PeriodFilter? LastSelectedFilter { get; set; }
    public DateTime? PickedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } set { this.MonthDatePicker.SelectedDate = value.ToMonth(); } }
    public override bool HasUpdates {
      get {
        return _hasUpdates;
      }
    }
    public EmployeeEarningSection Section { get { return EmployeeEarningSection.Loans; } }

    public EarningLoansControl() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
      this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth() ;
      //this.ItemsGridView.SortBy("RequestDate");
    }

    #region Loading
    private void EmployeeEarningLoansControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { }
    private void UserControlBase_Initialized(object sender, System.EventArgs e) { OnLoadData(); }
    private void FilterToggleButton_Click(object sender, RoutedEventArgs e) {
      if (this.FilterCurrentRadioButton.IsChecked == true)
        this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth() ;
      OnLoadData();
    }
    private void MonthDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (PickedMonth != null && PickedMonth != DateTime.Today.ToMonth())
        this.FilterSpecificRadioButton.IsChecked = true;
      OnLoadData();
    }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { OnLoadData(); }
    public void OnLoadData( bool force =false) {
      if (Employee == null || ParentTabSelected == false)
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
        Items = new ObservableCollection<EmployeeLoan>();
        return;
      }

      if (this.FilterSpecificRadioButton.IsChecked == true) {
        if (this.MonthDatePicker.SelectedDate != null)
          month = this.MonthDatePicker.SelectedDate.Value.ToMonth();
        else {
          Items = new ObservableCollection<EmployeeLoan>();
          return;
        }
      }

      if (false == force && filter == LastSelectedFilter && month == LastSelectedMonth) {
        return;
      }

      LastSelectedFilter = filter;
      LastSelectedMonth = month;

      this.SetBusy();
      EmployeesDataManager.GetLoans(Employee.EmployeeID.PackArray(), month, filter, false, OnDataLoaded);
    }

    private void ReloadLoansButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData(true);
    }

    private void OnDataLoaded(EmployeeLoan[] result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        //bool sort = (Items == null || Items.Count() == 0) && this.ItemsGridView.SortDescriptors.Count() == 0;
        Items = new ObservableCollection<EmployeeLoan>(result.OrderBy(s => s.RequestDate).ToArray());
        //if (sort)
        //  this.ItemsGridView.SortBy("RequestDate");
      } else
        Popup.AlertError(error);
    }
    #endregion

    #region Add / Edit Loan
    private void AddLoanButton_Click(object sender, RoutedEventArgs e) {
      OnNewItem();
    }

    private void OnNewItem() {
      LoanModifyWindow wnd = new LoanModifyWindow(Employee, LastSelectedMonth);
      this.DisplayModal<LoanModifyWindow>(wnd, OnModifyLoanClosed);
    }

    private void OnModifyLoanClosed(LoanModifyWindow wnd) {
      if (wnd.DialogResult != true)
        return;

      if (wnd.Result == OperationResult.Add) {
        Items.Add(wnd.UpdatedItem);
        this.ItemsGridView.SelectItem(wnd.UpdatedItem);
      } else if (wnd.Result == OperationResult.Delete)
        Items.Remove(wnd.OriginalItem);
      else if (wnd.Result == OperationResult.Update) {
        Items.Remove(wnd.OriginalItem);
        Items.Add(wnd.UpdatedItem);
        this.ItemsGridView.SelectItem(wnd.UpdatedItem);
      };

      _hasUpdates = true;
    }

    private void EditLoanButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnEditItem();
    }

    private void OnEditItem() {
      LoanModifyWindow wnd = new LoanModifyWindow(Employee,  (EmployeeLoan)this.ItemsGridView.SelectedItem);
      this.DisplayModal<LoanModifyWindow>(wnd, OnModifyLoanClosed);
    }

    private void DeleteLoanButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteItem();
    }

    private void OnDeleteItem() {
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        var itm = (EmployeeLoan)this.ItemsGridView.SelectedItem;

        EmployeesDataManager.DeleteLoan (itm, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeLoan result, Exception error) {
      if (error == null) {
        Items.Remove(result);
        _hasUpdates = true;
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    #endregion



    #region ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();

      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditItem();
    }

    private void NewMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnNewItem();
    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData(true);
    }

    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteItem();
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }
    #endregion
  }
}
