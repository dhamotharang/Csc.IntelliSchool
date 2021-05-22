using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;
using Csc.IntelliSchool.Modules.HRModule.Views.Structure;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class MonthlyCalculatorPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeTerminal> _items;
    #endregion

    #region Properties
    public ObservableCollection<EmployeeTerminal> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? SelectedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } }
    public int[] SelectedLists  { get{return this.ListsTreeView.CheckedItemsAs<EmployeeList>().Select(s=>s.ListID).ToArray(); } }
    #endregion

    // Constructors
    public MonthlyCalculatorPage() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadData();
    }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void LoadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData();
    }

    private void OnLoadData() {
      this.SetBusy();
      EmployeesDataManager.GetTerminals(OnDataLoaded);
      this.SetBusy();
      EmployeesDataManager.GetLists(OnListsLoaded);
    }

    private void OnListsLoaded(EmployeeList[] result, Exception error) {
      if (error == null) {
        this.ListsTreeView.ItemsSource = new EmployeeList[] { new EmployeeList() { Name = Csc.IntelliSchool.Assets.Resources.Text.Text_Unlisted } }.Concat(result).ToArray();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }
    private void OnDataLoaded(EmployeeTerminal[] result, Exception error) {
      if (error == null) {
        Items = new ObservableCollection<EmployeeTerminal>( result.OrderBy(s => s.Name).ToArray());
        //this.ItemsGridView.SortBy("Name");
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }
    #endregion
    
    #region ItemsContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("FetchMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(); }

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }

    #endregion

    #region Fetch
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnFetchItem(); }
    private void FetchButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnFetchItem();
    }

    private void FetchMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnFetchItem();
    }

    private void OnFetchItem() {
      var itm = (EmployeeTerminal)this.ItemsGridView.SelectedItem;

      if (itm.CanFetch == false) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_Unsupported);
        return;
      }

      TerminalFetchWindow wnd = new TerminalFetchWindow(itm);
      this.DisplayModal(wnd);
    }
    #endregion

    private void CalculateButton_Click(object sender, RoutedEventArgs e) {
      if (SelectedLists .Count() == 0 || SelectedMonth == null || (this.AttendanceCalculateCheckBox.IsChecked == false && this.EarningCalculateCheckBox.IsChecked == false)) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Error_Validation);
        return;
      }

      EmployeeRecalculateFlags flags = EmployeeRecalculateFlags.None;

      if (this.AttendanceCalculateCheckBox.IsChecked == true)
        flags |= EmployeeRecalculateFlags.Attendance;
      if (this.AttendanceEditedCheckBox.IsChecked == true)
        flags |= EmployeeRecalculateFlags.EditedAttendance;
      if (this.AttendanceLockedCheckBox.IsChecked == true)
        flags |= EmployeeRecalculateFlags.LockedAttendance ;

      if (this.EarningCalculateCheckBox.IsChecked == true)
        flags |= EmployeeRecalculateFlags.Earning;
      if (this.EarningBasicCheckBox.IsChecked == true)
        flags |= EmployeeRecalculateFlags.EarningSalariesOnly;
      if (this.EarningEditedCheckBox.IsChecked == true)
        flags |= EmployeeRecalculateFlags.EditedEarning;

      this.SetBusy();
      EmployeesDataManager.RecalculateMonthlyEarning(SelectedMonth.Value, SelectedLists.ToArray(), flags, OnCompleted);
    }

    private void OnCompleted(Exception error) {
      if (null == error)
        Popup.Alert(Csc.IntelliSchool.Assets.Resources.HumanResources.Alert_CalculatedSuccessfully);
      else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void ListsTreeView_ItemPrepared(object sender, RadTreeViewItemPreparedEventArgs e) {
      //if (((EmployeeList)e.PreparedItem.Item).ListID == 0)
      //  e.PreparedItem.CheckState = System.Windows.Automation.ToggleState.On;
    }


  }
}
