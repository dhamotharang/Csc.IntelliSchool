using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningAttendanceControl : Csc.Wpf.UserControlBase, IEarningControl {
    private ObservableCollection<EmployeeAttendance> _items;

    public Employee Employee { get { return DataContext as Employee; } }
    public ObservableCollection<EmployeeAttendance> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? CurrentMonth { get; set; }
    public DateTime? PickedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } set { this.MonthDatePicker.SelectedDate = value.ToMonth(); } }
    public EmployeeEarningSection Section { get { return EmployeeEarningSection.Attendance; } }

    public EarningAttendanceControl() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
      this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
    }

    #region Loading
    private void UserControlBase_Initialized(object sender, System.EventArgs e) {
      Items = new ObservableCollection<EmployeeAttendance>();
      OnLoadData();
    }
    private void MonthDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      OnLoadData();
    }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { return; }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { OnLoadData(); }
    public void OnLoadData(bool reload = false) {
      if (Employee == null || ParentTabSelected == false || (reload == false && PickedMonth == CurrentMonth))
        return;

      if (PickedMonth == null) {
        CurrentMonth = PickedMonth;
        Items = new ObservableCollection<EmployeeAttendance>();
        return;
      }

      this.SetBusy();
      EmployeesDataManager.GetAttendance(Employee.EmployeeID, PickedMonth.Value, OnDataLoaded);
    }



    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData(true);
    }

    private void OnDataLoaded(EmployeeAttendance[] result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        Items = new ObservableCollection<EmployeeAttendance>(result.OrderBy(s => s.Date).ToArray());
        //this.ItemsGridView.SortBy("DayNo");
        CurrentMonth = PickedMonth;
      } else
        Popup.AlertError(error);
    }
    #endregion




    #region ContextMenu

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }

    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      //menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.OnLoadData(true);
    }

    #endregion

    #region Recalculate
    private void RecalculateMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {

      OnRecalculate();
    }
    private void RecalculateButton_Click(object sender, RoutedEventArgs e) {
      OnRecalculate();
    }

    private void OnRecalculate() {
      if (Employee == null)
        return;

      if (CurrentMonth == null) {
        Items = new ObservableCollection<EmployeeAttendance>();
        return;
      }

      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Confirm_RecalculateAttendance, () => {
        this.SetBusy();
        EmployeesDataManager.RecalculateAttendance(Employee.EmployeeID, CurrentMonth.Value, OnDataLoaded);
      });
    }
    #endregion

    #region transactions
    private void ViewTransactionsButton_Click(object sender, RoutedEventArgs e) {
      OnViewTransactions();
    }

    private void OnViewTransactions() {
      if (Employee == null)
        return;

      if (CurrentMonth == null) {
        Items = new ObservableCollection<EmployeeAttendance>();
        return;
      }

      var wnd = new TerminalTransactionsWindow(this.Employee.Terminal.IP, this.Employee.TerminalUserID.Value, CurrentMonth.Value);
      this.DisplayModal(wnd);
    }

    private void ViewTransactionsMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnViewTransactions();
    }
    #endregion
  }
}



//#region Edit
//private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
//  OnEditItem();
//}

//private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
//  OnEditItem();
//}

//private void OnEditItem() {
//  AttendanceModifyWindow wnd = new AttendanceModifyWindow(Employee, (EmployeeAttendance)this.ItemsGridView.SelectedItem);
//  this.DisplayModal<AttendanceModifyWindow>(wnd, OnModifyWindowClosed);
//}

//private void OnModifyWindowClosed(AttendanceModifyWindow wnd) {
//  if (wnd.DialogResult != true)
//    return;

//  if (wnd.Result == OperationResult.Update) {
//    Items.Remove(wnd.OriginalItem);
//    Items.Add(wnd.Item);
//    this.ItemsGridView.SelectItem(wnd.Item);
//  };
//}
//#endregion