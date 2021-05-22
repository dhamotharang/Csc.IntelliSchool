using Csc.Components.Common;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Attendance {
  public partial class AttendanceSummaryPage : Csc.Wpf.PageBase {
    public enum AttendanceSummarySource {
      Earning = 0,
      Attendance
    }

    #region Fields
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private ObservableCollection<EmployeeAttendanceSummary> _items;
    private EmployeePosition[] _positions;
    #endregion

    #region Properties
    public ObservableCollection<EmployeeAttendanceSummary> AllItems { get; set; }
    public EmployeeBranch[] Branches { get { return _branches; } set { _branches = value; OnPropertyChanged(() => Branches); } }
    public DateTime? CurrentEndMonth { get; set; }
    public DateTime? CurrentStartMonth { get; set; }
    public AttendanceSummarySource? CurrentSource { get; set; }
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null; } }
    public EmployeeDepartment[] Departments { get { return _departments; } set { _departments = value; OnPropertyChanged(() => Departments); } }
    public ObservableCollection<EmployeeAttendanceSummary> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public int? ListID { get; set; }
    public EmployeePosition[] Positions { get { return _positions; } set { _positions = value; OnPropertyChanged(() => Positions); } }
    public int? SelectedBranchID {
      get {
        if (this.BranchComboBox.SelectedValue == null || (int)this.BranchComboBox.SelectedValue == 0)
          return null;

        return (int)this.BranchComboBox.SelectedValue;
      }
    }
    public AttendanceSummarySource SelectedSource { get { return (AttendanceSummarySource)this.SourceComboBox.SelectedIndex; }
      set { this.SourceComboBox.SelectedIndex = (int)value; } }
    public int? SelectedDepartmentID {
      get {
        if (this.DepartmentComboBox.SelectedValue == null || (int)this.DepartmentComboBox.SelectedValue == 0)
          return null;

        return (int)this.DepartmentComboBox.SelectedValue;
      }
    }
    public DateTime? SelectedEndMonth { get { return this.EndMonthDatePicker.SelectedDate.ToMonth(); } set { this.StartMonthDatePicker.SelectedDate = value.ToMonth(); } }
    public int? SelectedPositionID {
      get {
        if (this.PositionComboBox.SelectedValue == null || (int)this.PositionComboBox.SelectedValue == 0)
          return null;

        return (int)this.PositionComboBox.SelectedValue;
      }
    }
    public DateTime? SelectedStartMonth { get { return this.StartMonthDatePicker.SelectedDate.ToMonth(); } set { this.StartMonthDatePicker.SelectedDate = value.ToMonth(); } }
    #endregion

    // Constructors
    public AttendanceSummaryPage() {
      InitializeComponent();
      this.StartMonthDatePicker.SetPickerTypeMonth();
      this.EndMonthDatePicker.SetPickerTypeMonth();
    }

    #region Loading
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }

    private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnFilterData(); }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData();
    }

    private void MonthDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnLoadData(); }

    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter)
        OnFilterData();
    }

    private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { OnFilterData(); }

    private void OnDataLoaded(EmployeeAttendanceSummary[] result, Exception error) {
      if (error == null) {
        CurrentStartMonth = SelectedStartMonth;
        CurrentEndMonth = SelectedEndMonth;
        CurrentSource = SelectedSource;

        AllItems = new ObservableCollection<EmployeeAttendanceSummary>(result.OrderBy(s => s.Employee.Person.FullName));
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void OnFilterData() {
      if (AllItems == null)
        return;

      var lists = ListsComboBox.SelectedItems.Cast<EmployeeList>();
      if (AllItems.Count() > 0 && lists.Count() > 0) {
        var qry = AllItems.AsQueryable();

        if (lists.Count() != ListsComboBox.Items.Count) {
          int?[] listIds = lists.Select(s => s.ListID == 0 ? new int?() : s.ListID).ToArray();
          qry = qry.Where(s => listIds.Contains(s.Employee.ListID));
        }

        if (SelectedBranchID != null)
          qry = qry.Where(s => s.Employee.BranchID == SelectedBranchID);
        if (SelectedDepartmentID != null)
          qry = qry.Where(s => s.Employee.DepartmentID == SelectedDepartmentID);
        if (SelectedPositionID != null)
          qry = qry.Where(s => s.Employee.PositionID == SelectedPositionID);
        if (this.NameTextBox.Text.Length > 0)
          qry = qry.Where(s => s.Employee.Person.FullName.ToLower().Contains(this.NameTextBox.Text.ToLower()));

        Items = new ObservableCollection<EmployeeAttendanceSummary>(qry.ToArray());
      } else {
        Items = new ObservableCollection<EmployeeAttendanceSummary>();
      }

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
        //this.ItemsGridView.SortBy("Name");
      //}
      //_needsSorting = Items.Count() == 0;
    }

    private void OnGetBranchesCompleted(EmployeeBranch[] result, Exception error) {
      if (error == null) {
        Branches = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OnGetDepartmentsCompleted(EmployeeDepartment[] result, Exception error) {
      if (error == null) {
        Departments = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OnGetPositionsCompleted(EmployeePosition[] result, Exception error) {
      if (error == null) {
        Positions = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OnLoadData() {
      if (DataInitialized == false)
        return;

      if (this.SelectedStartMonth == null || this.SelectedEndMonth == null) {
        OnDataLoaded(new EmployeeAttendanceSummary[] { }, null);
        return;
      }

      this.SetBusy();
      if (SelectedSource == AttendanceSummarySource.Earning)
        EmployeesDataManager.GetEarningsAttendanceSummary(SelectedStartMonth.Value, SelectedEndMonth.Value, ListID != null ? ListID.Value.PackArray() : null, OnDataLoaded);
      else
        EmployeesDataManager.GetAttendanceSummary(SelectedStartMonth.Value, SelectedEndMonth.Value, ListID != null ? ListID.Value.PackArray() : null, OnDataLoaded);

    }

    private void OnLoadFilterData() {
      OnGetBranches();
      OnGetDepartments();
      OnGetPositions();
      OnGetLists();
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

    private void OnGetPositions() {
      this.SetBusy();
      EmployeesDataManager.GetPositions(false, ListID, OnGetPositionsCompleted);
    }

    private void OnGetDepartments() {
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, ListID, OnGetDepartmentsCompleted);
    }

    private void OnGetBranches() {
      // one for each call
      this.SetBusy();
      EmployeesDataManager.GetBranches(false, OnGetBranchesCompleted);
    }

    private void OnReload() {
      SelectedStartMonth = CurrentStartMonth;
      SelectedEndMonth = CurrentEndMonth;
      SelectedSource = CurrentSource ?? AttendanceSummarySource.Earning;
      OnLoadData();
    }

    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnReload();
    }
    #endregion

    #region Earning
    private void EarningButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEarning(); }

    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnEarning(); }
    private void OnEarning() {
      var earn = ((EmployeeAttendanceSummary)this.ItemsGridView.SelectedItem);
      var wnd = new Earning.EarningWindow(earn.Employee);
      this.DisplayModal < Earning.EarningWindow>(wnd, earn);
    }
    #endregion

    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {
      this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Position", "Absences", "Attendance");
    }


    #region ContextMenu
    private void EarningsMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEarning();
    }

    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("EarningsMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }
   
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnReload();
    }


    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }

    #endregion


    #region Print
    private void PrintButton_Click(object sender, RoutedEventArgs e) {
      if (AllItems == null || AllItems.Count() == 0 || this.CurrentStartMonth == null || this.CurrentEndMonth == null)
        return;

      EmployeeFilterWindow wnd = new EmployeeFilterWindow();
      wnd.Title = string.Format("{0:y} to {1:y}", CurrentStartMonth, CurrentEndMonth);
      wnd.GridView = this.ItemsGridView;
      wnd.ListID = ListID;
      this.DisplayModal<EmployeeFilterWindow>(wnd, OnPrintAttndance);
    }


    private void OnPrintAttndance(EmployeeFilterWindow obj) {
      if (obj.DialogResult != true)
        return;

      var items = obj.ItemsAs<EmployeeAttendanceSummary>().ToArray();
      this.DisplayReport(new Reports.AttendanceSummaryReport() {
        StartMonth = this.CurrentStartMonth.Value,
        EndMonth = this.CurrentEndMonth.Value
      }, items.Select(s => (EmployeeAttendanceSummary)s)
         .GroupBy(s => s.Employee.Department).OrderBy(s => s.Key != null ? s.Key.Name : null).ToArray(), null);
    }
    #endregion

  }
}
