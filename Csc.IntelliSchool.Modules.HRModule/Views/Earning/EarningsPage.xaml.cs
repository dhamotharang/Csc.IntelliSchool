using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Collections.ObjectModel;
using System.Windows;
using Csc.IntelliSchool.Modules.HRModule.Views.Earning;
using System.IO;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningsPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeEarning> _items;
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private EmployeePosition[] _positions;
    //private bool _needsSorting = true;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null; } }
    public ObservableCollection<EmployeeEarning> AllItems { get; set; }
    public ObservableCollection<EmployeeEarning> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public EmployeeBranch[] Branches { get { return _branches; } set { _branches = value; OnPropertyChanged(() => Branches); } }
    public EmployeeDepartment[] Departments { get { return _departments; } set { _departments = value; OnPropertyChanged(() => Departments); } }
    public EmployeePosition[] Positions { get { return _positions; } set { _positions = value; OnPropertyChanged(() => Positions); } }
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
    public DateTime? CurrentMonth { get; set; }
    public DateTime? SelectedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } }

    public int? ListID { get; set; }
    #endregion

    // Constructors
    public EarningsPage() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
      // this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData(true);
    }
    private void MonthDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnLoadData(); }

    private void OnLoadFilterData() {
      OnGetBranches();
      OnGetDepartments();
      OnGetPositions();
      OnGetLists();
    }

    private void OnGetBranches() {
      // one for each call
      this.SetBusy();
      EmployeesDataManager.GetBranches(false, OnGetBranchesCompleted);
    }

    private void OnGetDepartments() {
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, ListID, OnGetDepartmentsCompleted);
    }

    private void OnGetPositions() {
      this.SetBusy();
      EmployeesDataManager.GetPositions(false, ListID, OnGetPositionsCompleted);
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
   

    private void OnLoadData(bool reload = false) {
      if (DataInitialized == false || (reload == false && SelectedMonth == CurrentMonth))
        return;

      if (this.SelectedMonth == null) {
        CurrentMonth = SelectedMonth;
        OnDataLoaded(new EmployeeEarning[] { }, null);
        return;
      }
      
      this.SetBusy();
      EmployeesDataManager.GetListEarnings(SelectedMonth.Value, ListID != null ? ListID.Value.PackArray() : null, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeEarning[] result, Exception error) {
      if (error == null) {
        this.CurrentMonth = this.SelectedMonth;
        AllItems = new ObservableCollection<EmployeeEarning>(result.OrderBy(s => s.Employee.Person.FullName));
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter)
        OnFilterData();
    }
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }

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

        Items = new ObservableCollection<EmployeeEarning>(qry.ToArray());
      }       else
        Items = new ObservableCollection<EmployeeEarning>();

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("Name");
      //}

      //_needsSorting = Items.Count() == 0;
    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    #region Earning
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnEarning(); }

    private void EarningButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEarning(); }

    private void OnEarning() {
      var earn = ((EmployeeEarning)this.ItemsGridView.SelectedItem);
      var wnd = new Earning.EarningWindow(earn.Employee, SelectedMonth);
      this.DisplayModal < EarningWindow>(wnd, earn, OnEarningWindowClosed);
    }

    private void OnEarningWindowClosed(EarningWindow wnd, object state) {
      if (wnd.SummaryControl.UpdatedMonths.Contains(CurrentMonth.Value) == false)
        return;

      var earn = (EmployeeEarning)state;
      AllItems.Remove(earn);
      this.SetBusy();
      EmployeesDataManager.GetEarnings(SelectedMonth.Value,  earn.EmployeeID , OnGetSingleEarning);

    }

    private void OnGetSingleEarning(EmployeeEarning result, Exception error) {
      if (error == null) {
        AllItems.Add(result);
        OnFilterData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Recalculate
    private void RecalculateBasicButton_Click(object sender, RoutedEventArgs e) {
      if (DataInitialized == false || SelectedMonth == null)
        return;

      EmployeeFilterWindow wnd = new EmployeeFilterWindow();
      wnd.Title = string.Format(SelectedMonth.Value.ToString("y")) + " - Basic";
      wnd.GridView = this.ItemsGridView;
      wnd.ListID = ListID;
      this.DisplayModal<EmployeeFilterWindow>(wnd, OnCalculateBasicEarnings);
    }

    private void OnCalculateBasicEarnings(EmployeeFilterWindow obj) {
      if (obj.DialogResult != true)
        return;

      this.SetBusy();
      var items = obj.ItemsAs<EmployeeEarning>().Select(s=>s.EmployeeID).ToArray();
      EmployeesDataManager.RecalculateEarnings(SelectedMonth.Value, items, EmploeeEarningCalculationMode.Basic, OnRecalculateCompleted);
    }

    private void RecalculateFullMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      if (DataInitialized == false || SelectedMonth == null)
        return;

      EmployeeFilterWindow wnd = new EmployeeFilterWindow();
      wnd.Title = string.Format(SelectedMonth.Value.ToString("y")) + " - Full";
      wnd.GridView = this.ItemsGridView;
      wnd.ListID = ListID;
      this.DisplayModal<EmployeeFilterWindow>(wnd, OnCalculateFullEarnings);
    }


    private void OnCalculateFullEarnings(EmployeeFilterWindow obj) {
      if (obj.DialogResult != true)
        return;

      this.SetBusy();
      var items = obj.ItemsAs<EmployeeEarning>().ToArray();
      EmployeesDataManager.RecalculateEarnings(SelectedMonth.Value, items.Select(s => s.EmployeeID).ToArray(), EmploeeEarningCalculationMode.Full, OnRecalculateCompleted);
    }


    private void OnRecalculateCompleted(EmployeeEarning[] result, Exception error) {
      if (result != null) {
        var updatedEmployeeIDs = result.Select(s => s.EmployeeID).ToArray();
        foreach (var itm in AllItems.Where(s => updatedEmployeeIDs.Contains(s.EmployeeID)).ToArray())
          AllItems.Remove(itm);

        foreach (var itm in result)
          AllItems.Add(itm);

        OnFilterData();
      } else {
        Popup.AlertError(error);
      }
      this.ClearBusy();
    }
    #endregion

    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {
      this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Position", "Salary", "Net");
    }

    #region Print
    private void PrintButton_Click(object sender, RoutedEventArgs e) {
      if (DataInitialized == false || SelectedMonth == null)
        return;

      EmployeeFilterWindow wnd = new EmployeeFilterWindow();
      wnd.Title = string.Format("{0:y}", CurrentMonth);
      wnd.GridView = this.ItemsGridView;
      wnd.ListID = ListID;
      this.DisplayModal<EmployeeFilterWindow>(wnd, OnPrintEarnings);
    }

    private void OnPrintEarnings(EmployeeFilterWindow obj) {
      if (obj.DialogResult != true)
        return;

      var items = obj.ItemsAs<EmployeeEarning>().Where(s => s.Employee.Salary.HideFromReports == false).ToArray();
      this.DisplayReport(new Reports.EarningsReport() {
        Month = this.CurrentMonth,
      }, items.Select(s => (EmployeeEarning)s)
         .GroupBy(s => s.Employee.Department).OrderBy(s => s.Key != null ? s.Key.Name : null).ToArray(), null);
    }
    #endregion



    #region ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();



      menu.FindMenuItem("EarningsMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }


    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }


    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData(true);
    }


    private void EarningsMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEarning();
    }


    #endregion

  }
}
