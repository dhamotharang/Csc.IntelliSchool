using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class VacationsPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeVacation> _items;
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private EmployeePosition[] _positions;
    private DateTime? _selectedStartDate;
    private DateTime? _selectedEndDate;
    private EmployeeVacationType[] _types;
    //private bool _needsSorting = true;
    private int? _listID;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null && Types != null; } }
    public ObservableCollection<EmployeeVacation> AllItems { get; set; }
    public ObservableCollection<EmployeeVacation> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public EmployeeBranch[] Branches { get { return _branches; } set { _branches = value; OnPropertyChanged(() => Branches); } }
    public EmployeeDepartment[] Departments { get { return _departments; } set { _departments = value; OnPropertyChanged(() => Departments); } }
    public EmployeePosition[] Positions { get { return _positions; } set { _positions = value; OnPropertyChanged(() => Positions); } }
    public EmployeeVacationType[] Types { get { return _types; } set { if (_types != value) { _types = value; OnPropertyChanged(() => Types); } } }


    public int? SelectedBranchID
    {
      get
      {
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
    public DateTime CurrentStartDate { get; set; }
    public DateTime CurrentEndDate { get; set; }
    public int[] CurrentTypeIDs { get; set; }
    public DateTime? SelectedStartDate { get { return _selectedStartDate; } set { if (_selectedStartDate.ToDay() != value) { _selectedStartDate = value.ToDay(); OnPropertyChanged(() => SelectedStartDate); } } }
    public DateTime? SelectedEndDate { get { return _selectedEndDate; } set { if (_selectedEndDate.ToDay() != value) { _selectedEndDate = value.ToDay(); OnPropertyChanged(() => SelectedEndDate); } } }
    public int[] SelectedTypeIDs {
      get {
        return this.TypeComboBox.GetSelectedItemIDs<EmployeeVacationType>(a => a.TypeID);
      }
      set {
        this.TypeComboBox.SetSelectedItemIDs<EmployeeVacationType>(value ?? new int[] { }, a => a.TypeID);
      }
    }




    public int? ListID { get { return _listID; } set { _listID = value; OnPropertyChanged(() => ListID); } }
    #endregion

    // Constructors
    public VacationsPage() {
      InitializeComponent();
      this.StartDatePicker.SelectedDate = CurrentStartDate = DateTime.Today.ToYear();
      this.EndDatePicker.SelectedDate = CurrentEndDate = DateTime.Today.ToYearEnd();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void LoadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(true); }

    private void OnLoadFilterData() {
      // one for each call
      OnGetBranches();
      OnGetDepartments();
      OnGetPositions();
      OnGetLists();
      OnGetVacationTypes();
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

    private void OnGetVacationTypes() {
      this.SetBusy();
      EmployeesDataManager.GetVacationTypes(false, OnGetTypes);
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
    private void OnGetTypes(EmployeeVacationType[] result, Exception error) {
      if (error == null) {
        Types = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        this.TypeComboBox.SelectedIndex = 0;
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }


    private void OnLoadData(bool reload = false) {
      if (DataInitialized == false)
        return;

      if (reload) {
        SelectedStartDate = CurrentStartDate;
        SelectedEndDate = CurrentEndDate;
        SelectedTypeIDs = CurrentTypeIDs;
      }

      if (this.LoadPanel.Validate(true) == false)
        return;

      if (SelectedTypeIDs.Count() == 0) {
        OnDataLoaded(new EmployeeVacation[] { }, null);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.GetVacations(SelectedTypeIDs,
        SelectedStartDate.Value, SelectedEndDate.Value,
        null, ListID != null ? ListID.Value.PackArray() : null, true, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeVacation[] result, Exception error) {
      this.CurrentStartDate = SelectedStartDate.Value;
      this.CurrentEndDate = SelectedEndDate.Value;

      if (error == null) {
        if (SelectedTypeIDs.Contains(0))
          CurrentTypeIDs = new int[] { 0 };
        else
          CurrentTypeIDs = SelectedTypeIDs;
        AllItems = new ObservableCollection<EmployeeVacation>(result);
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

    private void OnFilterData(object itm = null) {
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

          qry = qry.Where(s => s.Employee.Person.FullName.ToLower().Contains(filterText));
        }


        Items = new ObservableCollection<EmployeeVacation>(qry.ToArray());
      } else
        Items = new ObservableCollection<EmployeeVacation>();


      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("Date", "FullName");
      //}
      //_needsSorting = Items.Count() == 0;

      if (itm != null)
        this.ItemsGridView.SelectItem(itm);
      else if (this.ItemsGridView.Items.Count > 0)
        this.ItemsGridView.SelectItem(this.ItemsGridView.Items[0]);
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
      var itm = (EmployeeVacation)this.ItemsGridView.SelectedItem;
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        EmployeesDataManager.DeleteVacation(itm, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeVacation result, Exception error) {
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
    private void AddButton_Click(object sender, RoutedEventArgs e) { OnAddItem(); }

    private void OnAddItem() {
      EmployeeSelectionWindow wnd = new EmployeeSelectionWindow(EmployeeSelectionType.Default, CurrentStartDate.ToMonth());
      wnd.ParamListID = ListID;
      this.DisplayModal<EmployeeSelectionWindow>(wnd, OnEmployeeSelected);
    }

    private void OnEmployeeSelected(EmployeeSelectionWindow obj) {
      if (obj.DialogResult != true)
        return;

      VacationModifyWindow wnd = new VacationModifyWindow(obj.SelectedEmployee);
      this.DisplayModal<VacationModifyWindow>(wnd, OnModifyWindowClosed);
    }

    private void OnModifyWindowClosed(VacationModifyWindow obj) {
      if (obj.Result == OperationResult.None)
        return;

      bool validItem = DateTimeExtensions.Overlaps(obj.Item.StartDate, obj.Item.EndDate, CurrentStartDate , CurrentEndDate ) &&
        (CurrentTypeIDs.Contains(0) || (obj.Item.TypeID != null && CurrentTypeIDs.Contains(obj.Item.TypeID.Value)));

      if (obj.Result == OperationResult.Add && validItem) {
        AllItems.Add(obj.Item);
        OnFilterData(obj.Item);
      } else if (obj.Result == OperationResult.Update) {
        AllItems.Remove(obj.OriginalItem);
        if (validItem) {
          AllItems.Add(obj.Item);
          OnFilterData(obj.Item);
        }
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
      var itm = (EmployeeVacation)this.ItemsGridView.SelectedItem;
      VacationModifyWindow wnd = new VacationModifyWindow(itm.Employee, itm);
      this.DisplayModal<VacationModifyWindow>(wnd, OnModifyWindowClosed);
    }
    #endregion



  }
}
