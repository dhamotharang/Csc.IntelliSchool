using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using System.Collections.ObjectModel;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class OfficialDocumentsPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeOfficialDocumentSummary> _items;
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private EmployeePosition[] _positions;
    private bool _needsSorting = true;
    private int? _listID;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null; } }
    public ObservableCollection<EmployeeOfficialDocumentSummary> AllItems { get; set; }
    public ObservableCollection<EmployeeOfficialDocumentSummary> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
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
    public int? ListID { get { return _listID; } set { _listID = value; OnPropertyChanged(() => ListID); } }
    #endregion

    // Constructors
    public OfficialDocumentsPage() {
      InitializeComponent();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void MonthDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnLoadData(); }

    private void OnLoadFilterData() {
      // one for each call
      OnGetBranches();
      OnGetDepartments();
      OnGetPositions();
      OnGetLists();
    }

    private void OnGetBranches() {
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

    private void OnLoadData() {
      if (DataInitialized == false)
        return;


      this.SetBusy();
      EmployeesDataManager.GetOfficialDocumentSummary(
        DateTime.Today.ToMonth(), 
        ListID != null ? ListID.Value.PackArray() : null, 
        null, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeOfficialDocumentSummary[] result, Exception error) {
      if (error == null) {
        AllItems = new ObservableCollection<EmployeeOfficialDocumentSummary>(result.OrderBy(s => s.FullName));
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter) {
        OnFilterData();
      }
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
          qry = qry.Where(s => listIds.Contains(s.ListID));
        }

        if (SelectedBranchID != null)
          qry = qry.Where(s => s.BranchID == SelectedBranchID);
        if (SelectedDepartmentID != null)
          qry = qry.Where(s => s.DepartmentID == SelectedDepartmentID);
        if (SelectedPositionID != null)
          qry = qry.Where(s => s.PositionID == SelectedPositionID);
        if (this.NameTextBox.Text.Length > 0) {
          string filterText = this.NameTextBox.Text.ToLower();

          qry = qry.Where(s => s.FullName.ToLower().Contains(filterText));
        }

        Items = new ObservableCollection<EmployeeOfficialDocumentSummary>(qry.ToArray());
      }
      else
        Items = new ObservableCollection<EmployeeOfficialDocumentSummary>();

      if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
        this.ItemsGridView.SortBy(EmployeeOfficialDocumentSummary.Key_FullName);
      }

      _needsSorting = Items.Count() == 0;

    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    private void ItemsGridView_AutoGeneratingColumn(object sender, Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e) {
      e.FormatDynamicColumn();
    }

    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      e.SelectParentRow<EmployeeOfficialDocumentSummary>();
      OnEditEmployee();
    }

    private void OnEditEmployee() {
      this.SetBusy();
      EmployeesDataManager.GetSingleEmployee(((EmployeeOfficialDocumentSummary)this.ItemsGridView.SelectedItem).EmployeeID, DateTime.Today, false,  OnGetEmployee);
    }

    private void OnGetEmployee(Employee result, Exception error) {
      this.ClearBusy();
      if (result != null) {
        EmployeeModifyWindow wnd = new EmployeeModifyWindow(result, EmployeeModificationSection.Basic);
        this.DisplayModal<EmployeeModifyWindow>(wnd, OnModifyWindowClosed);
      } else if (error != null){
        Popup.AlertError(error);
      }
    }

    private void OnModifyWindowClosed(EmployeeModifyWindow obj) {
      if (obj.DialogResult != true)
        return;

      this.SetBusy();
      EmployeesDataManager.GetOfficialDocumentSummary(DateTime.Today, null, new int[] { obj.Item.EmployeeID }, OnSingleSummaryLoaded);
    }

    private void OnSingleSummaryLoaded(EmployeeOfficialDocumentSummary[] result, Exception error) {
      if (result != null && result.Count() > 0) {
        var itm = result.First();
        AllItems.Remove(AllItems.Single(s => s.EmployeeID == itm.EmployeeID));
        AllItems.Add(itm);
        OnFilterData();
        this.ItemsGridView.SelectItem(itm);
      } else if (error != null)
        Popup.AlertError(error);

      this.ClearBusy();
    }

    #region ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }



    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData();
    }


    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditEmployee();
    }
    #endregion
  }
}
