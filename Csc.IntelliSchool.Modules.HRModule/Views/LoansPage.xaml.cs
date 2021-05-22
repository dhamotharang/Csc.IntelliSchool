using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class LoansPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeLoan> _items;
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private EmployeePosition[] _positions;
    //private bool _needsSorting = true;
    private int? _listID;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null; } }
    public ObservableCollection<EmployeeLoan> AllItems { get; set; }
    public ObservableCollection<EmployeeLoan> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public EmployeeBranch[] Branches { get { return _branches; } set { _branches = value; OnPropertyChanged(() => Branches); } }
    public EmployeeDepartment[] Departments { get { return _departments; } set { _departments = value; OnPropertyChanged(() => Departments); } }
    public EmployeePosition[] Positions { get { return _positions; } set { _positions = value; OnPropertyChanged(() => Positions); } }

    public int? SelectedBranchID
    {
      get
      {
        if (this.BranchComboBox.SelectedValue == null || (int)this.BranchComboBox.SelectedValue == 0)
          return null;
        return (int)this.BranchComboBox.SelectedValue;
      }
    }
    public int? SelectedDepartmentID
    {
      get
      {
        if (this.DepartmentComboBox.SelectedValue == null || (int)this.DepartmentComboBox.SelectedValue == 0)
          return null;

        return (int)this.DepartmentComboBox.SelectedValue;
      }
    }
    public int? SelectedPositionID
    {
      get
      {
        if (this.PositionComboBox.SelectedValue == null || (int)this.PositionComboBox.SelectedValue == 0)
          return null;

        return (int)this.PositionComboBox.SelectedValue;
      }
    }
    public DateTime? CurrentStartDate { get; set; }
    public DateTime? CurrentEndDate { get; set; }
    public DateTime? SelectedStartDate { get { return this.StartDatePicker.SelectedDate.ToMonth(); } set { this.StartDatePicker.SelectedDate = value.ToMonth(); } }
    public DateTime? SelectedEndDate { get { return this.EndDatePicker.SelectedDate.ToMonth(); } set { this.EndDatePicker.SelectedDate = value.ToMonth(); } }

    public int? ListID { get { return _listID; } set { _listID = value; OnPropertyChanged(() => ListID); } }
    #endregion

    // Constructors
    public LoansPage() {
      InitializeComponent();
      this.StartDatePicker.SetPickerTypeMonth();
      this.EndDatePicker.SetPickerTypeMonth();
      this.StartDatePicker.SelectedDate = CurrentStartDate = DateTime.Today.ToMonth();
      this.EndDatePicker.SelectedDate = CurrentEndDate = DateTime.Today.ToMonth();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void LoadButton_Click(object sender, RoutedEventArgs e) {       OnLoadData();     }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {  OnLoadData(true); }

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

    private void OnLoadData(bool reload  = false) {
      if (DataInitialized == false)
        return;

      if (reload ) {
        SelectedStartDate = CurrentStartDate;
        SelectedEndDate = CurrentEndDate;
      }

      if (this.LoadPanel.Validate(true) == false)
        return;

      this.SetBusy();

      EmployeesDataManager.GetLoans(SelectedStartDate.Value.ToMonth(), SelectedEndDate.Value.ToMonth(), null, ListID != null ? ListID.Value.PackArray() : null,  true, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeLoan[] result, Exception error) {
      if (error == null) {
        this.CurrentStartDate = SelectedStartDate;
        this.CurrentEndDate = SelectedEndDate;
        AllItems = new ObservableCollection<EmployeeLoan>(result.OrderBy(s => s.RequestDate));
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

    private void OnFilterData(object itm  = null) {
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
        if (this.NameTextBox.Text.Length > 0) {
          string filterText = this.NameTextBox.Text.ToLower();

          qry = qry.Where(s => s.Employee.Person.FullName.ToLower().Contains(filterText) );
        }

        Items = new ObservableCollection<EmployeeLoan>(qry.ToArray());
      } else
        Items = new ObservableCollection<EmployeeLoan>();

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("RequestDate", "FullName");
      //}
      //_needsSorting = Items.Count() == 0;

      if (itm != null)
      this.ItemsGridView.SelectItem(itm);
    }

    #endregion

    #region Menu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      (sender as FrameworkElement).OpenContextMenu();
    }

    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteItem();
    }

    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteItem();
    }

    private void OnDeleteItem() {
      var itm = (EmployeeLoan)this.ItemsGridView.SelectedItem;
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        EmployeesDataManager.DeleteLoan (itm, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeLoan result, Exception error) {
      if (error == null) {
        AllItems.Remove(result);
        Items.Remove(result);
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    #endregion


    #region Add
    private void NewMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnAddItem(); }
    private void AddButton_Click(object sender, RoutedEventArgs e) {  OnAddItem(); }

    private void OnAddItem() {
      EmployeeSelectionWindow wnd = new EmployeeSelectionWindow(EmployeeSelectionType.Default, CurrentStartDate.ToMonth());
      wnd.ParamListID = ListID;
      this.DisplayModal<EmployeeSelectionWindow>(wnd, OnEmployeeSelected);
    }

    private void OnEmployeeSelected(EmployeeSelectionWindow obj) {
      if (obj.DialogResult != true)
        return;

      LoanModifyWindow wnd = new LoanModifyWindow(obj.SelectedEmployee, CurrentStartDate.ToMonth());
      this.DisplayModal<LoanModifyWindow>(wnd, OnModifyWindowClosed);
    }

    private void OnModifyWindowClosed(LoanModifyWindow obj) {
      if (obj.Result == OperationResult.None)
        return;


      if (obj.Result == OperationResult.Add && obj.UpdatedItem.RequestDate >= SelectedStartDate && obj.UpdatedItem.RequestDate <= SelectedEndDate ) {
        AllItems.Add(obj.UpdatedItem);
        OnFilterData(obj.UpdatedItem);
      } else if (obj.Result == OperationResult.Update && obj.UpdatedItem.RequestDate >= SelectedStartDate && obj.UpdatedItem.RequestDate <= SelectedEndDate) {
        AllItems.Remove(obj.OriginalItem);
        AllItems.Add(obj.UpdatedItem);
        OnFilterData(obj.UpdatedItem);
      } else if (obj.Result == OperationResult.Delete) {
        AllItems.Remove(obj.OriginalItem);
        Items.Remove(obj.OriginalItem);
      }
    }
    #endregion

    #region Edit
    private void EditButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnEditItem();
    }
    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditItem();
    }
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      OnEditItem();
    }

    private void OnEditItem() {

      var itm = (EmployeeLoan)this.ItemsGridView.SelectedItem;
      LoanModifyWindow wnd = new LoanModifyWindow(itm.Employee, itm);
      this.DisplayModal<LoanModifyWindow>(wnd, OnModifyWindowClosed);
    }
    #endregion



  }
}
