using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;
using Csc.IntelliSchool.Modules.HRModule.Assets.Helpers;
using Csc.Wpf.Views;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Attendance {
  public partial class AttendanceMatrixPage : Csc.Wpf.PageBase {
    #region Fields
    //private bool _needsSorting = true;
    private ObservableCollection<EmployeeAttendanceObject> _items;
    #endregion

    #region Properties
    public EmployeeAttendanceObject[] AllItems { get; set; }
    public EmployeeAttendanceValueConverter Converter { get { return this.Resources["ValueConverter"] as EmployeeAttendanceValueConverter; } }
    public ObservableCollection<EmployeeAttendanceObject> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public int? SelectedBranchID {
      get {
        if (this.BranchComboBox.SelectedValue == null || (int)this.BranchComboBox.SelectedValue == 0)
          return null;

        return (int)this.BranchComboBox.SelectedValue;
      }
    }
    public int? SelectedDepartmentID {
      get {
        if (this.DepartmentComboBox.SelectedValue == null || (int)this.DepartmentComboBox.SelectedValue == 0)
          return null;

        return (int)this.DepartmentComboBox.SelectedValue;
      }
    }
    public int? SelectedPositionID {
      get {
        if (this.PositionComboBox.SelectedValue == null || (int)this.PositionComboBox.SelectedValue == 0)
          return null;

        return (int)this.PositionComboBox.SelectedValue;
      }
    }
    public int? ListID { get; set; }
    public DateTime? SelectedMonth { get { return this.MonthDatePicker.SelectedDate != null ? this.MonthDatePicker.SelectedDate.Value.ToMonth() : new DateTime?(); } }
    #endregion

    // Constructors
    public AttendanceMatrixPage() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void LoadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData();
    }

    private void OnLoadFilterData() {
      OnGetBranches();
      OnGetDepartments();
      OnGetPositions();
      OnGetLists();
    }

    private void OnGetBranches() {
      this.SetBusy();
      EmployeesDataManager.GetBranches(false, (res, err) => {
        this.BranchComboBox.FillAsyncItems(res, err, a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All, null);
        this.ClearBusy();
      });
    }

    private void OnGetDepartments() {
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, ListID, (res, err) => {
        this.DepartmentComboBox.FillAsyncItems(res, err, a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All, null);
        this.ClearBusy();
      });
    }

    private void OnGetPositions() {
      this.SetBusy();
      EmployeesDataManager.GetPositions(false, ListID, (res, err) => {
        this.PositionComboBox.FillAsyncItems(res, err, a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All, null);
        this.ClearBusy();
      });
    }

    private void OnGetLists() {
      this.SetBusy();
      EmployeesDataManager.GetLists((res, err) => {
        Action<EmployeeList> nullCallback = null;
        if (ListID == 0 || ListID == null) {
          nullCallback = s => s.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_Unlisted;
        }
        this.ListsComboBox.FillAsyncItems(res, err, nullCallback, this);
        this.ListsComboBox.SelectAll();
      });
    }

    private void OnLoadData() {
      if (SelectedMonth == null) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoSelectedMonth);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.GetAttendance( SelectedBranchID , SelectedDepartmentID, SelectedPositionID,
        this.ListsComboBox.SelectedItems.Cast<EmployeeList>().Select(s=>s.ListID).ToArray(), SelectedMonth.Value, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeAttendanceObject[] result, Exception error) {
      if (error == null) {
        AllItems = result;
        FixGridViewColumns();
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void FixGridViewColumns() {
      var dates = SelectedMonth.Value.GetMonthDays().ToArray();
      int idx = 0;
      foreach (var grp in this.ItemsGridView.ColumnGroups.Skip(3)) {
        if (idx >= dates.Count())
          grp.Header = string.Empty;
        else
          grp.Header = string.Format("{0:dd} {0:ddd}", dates[idx]);
        idx++;
      }
    }

    private void SetColumnVisibility() {
      foreach (var col in this.ItemsGridView.Columns) {
        var grpHeader = this.ItemsGridView.ColumnGroups.Single(s => s.Name == col.ColumnGroupName).Header;
        var grpAvailable = AllItems == null ||  (grpHeader != null && grpHeader.ToString() != "");

        if (col.Header.ToString() == "In")
          col.IsVisible = this.InCheckBox.IsChecked == true && grpAvailable;
        if (col.Header.ToString() == "Out")
          col.IsVisible = this.OutCheckBox.IsChecked == true && grpAvailable;
        if (col.Header.ToString() == "TO")
          col.IsVisible = this.TimeOffCheckBox.IsChecked == true && grpAvailable;
      }
    }

    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter) {
        OnFilterData();
      }
    }
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }


    private void OnFilterData() {
      if (AllItems == null)
        return;

      var qry = AllItems.AsQueryable();

      if (this.NameTextBox.Text.Length > 0) {
        string filterText = this.NameTextBox.Text.ToLower();

        qry = qry.Where(s => s.Employee.Person.FullName.ToLower().Contains(filterText) );
      }

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  SortDefault();
      //}

      Items = new ObservableCollection<EmployeeAttendanceObject>(qry.ToArray());
    }
    #endregion


    #region ItemsContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();



    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(); }
    #endregion

    private void ExportButton_Click(object sender, RoutedEventArgs e) {
      this.ItemsGridView.ExportAsync();
    }


    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {
      this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Position");
    }

    private void ColumnCheckBox_Click(object sender, RoutedEventArgs e) {
      SetColumnVisibility();
    }

    private void DetailsCheckBox_Click(object sender, RoutedEventArgs e) {
      Converter.Detailed = this.DetailsCheckBox.IsChecked == true;
      this.ItemsGridView.Rebind();
    }


  }
}
