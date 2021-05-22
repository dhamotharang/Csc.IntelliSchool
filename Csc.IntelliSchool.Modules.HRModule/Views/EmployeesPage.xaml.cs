using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeesPage : Csc.Wpf.PageBase {
    #region Nested Types
    enum EmployeesViewType {
      Details,
      Salaries,
      Bank,
      Terminations
    }

    class MenuItems {
      public static readonly string NewEmployeeMenuItem = "NewEmployeeMenuItem";
      public static readonly string NewEmployeeSeparatorMenuItem = "NewEmployeeSeparatorMenuItem";
      public static readonly string EditEmployeeMenuItem = "EditEmployeeMenuItem";
      public static readonly string AddDependantMenuItem = "AddDependantMenuItem";
      public static readonly string EmployeeEarningMenuItem = "EmployeeEarningMenuItem";
      public static readonly string EmployeeDocumentsMenuItem = "EmployeeDocumentsMenuItem";
      public static readonly string EditDependantMenuItem = "EditDependantMenuItem";
      public static readonly string DeleteDependantMenuItem = "DeleteDependantMenuItem";
    }

    class GridColumns {
      public static readonly string[] TerminationReason = new string[] { "TerminationReason" };
      public static readonly string[] DependantsCount = new string[] { "DependantsCount" };
    }

    class GridColumnGroups {
      public static readonly string[] Salary = new string[] { "Salary" };
      public static readonly string[] Bank = new string[] { "Bank" };
      public static readonly string[] Education = new string[] { "Education", "ArabicEducation" };
    }
    #endregion

    #region Fields
    private ObservableCollection<Employee> _items;
    private EmployeeBranch[] _branches;
    private EmployeeDepartment[] _departments;
    private EmployeePosition[] _positions;
    private int? _listID;
    private EmployeesViewType _viewType;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Branches != null && Departments != null && Positions != null; } }
    public ObservableCollection<Employee> AllItems { get; set; }
    public ObservableCollection<Employee> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
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
    #endregion

    #region Parameters
    public string Type {
      get { return ViewType.ToString(); }
      set {
        EmployeesViewType type = EmployeesViewType.Details;
        Enum.TryParse(value, out type);
        SetViewType(type);
      }
    }
    private EmployeesViewType ViewType { get { return _viewType; }}

    public int? ListID { get { return _listID; } set { _listID = value; OnPropertyChanged(() => ListID); } }

    public bool CanEnroll { get { return ViewType != EmployeesViewType.Bank; } }
    public bool ShowCurrent { get { return ViewType != EmployeesViewType.Terminations; } }
    public bool ShowSalary { get { return ViewType == EmployeesViewType.Terminations || ViewType == EmployeesViewType.Salaries; } }
    public bool ShowBank { get { return ViewType == EmployeesViewType.Terminations || ViewType == EmployeesViewType.Bank; } }
    public bool ShowEducation { get { return ViewType == EmployeesViewType.Terminations || ViewType == EmployeesViewType.Details; } }
    public bool ShowDependants { get { return ViewType == EmployeesViewType.Terminations || ViewType == EmployeesViewType.Details; } }
    #endregion

    public EmployeesPage() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
      this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
    }

    #region Loading
    private void SetViewType(EmployeesViewType type) {
      _viewType = type;

      if (type != EmployeesViewType.Terminations)
        this.ItemsGridView.RemoveColumns(GridColumns.TerminationReason);
      if (type != EmployeesViewType.Terminations && type != EmployeesViewType.Salaries)
        this.ItemsGridView.RemoveColumnGroups(GridColumnGroups.Salary);
      if (type != EmployeesViewType.Terminations && type != EmployeesViewType.Bank)
        this.ItemsGridView.RemoveColumnGroups(GridColumnGroups.Bank);
      if (type != EmployeesViewType.Terminations && type != EmployeesViewType.Details) {
        this.ItemsGridView.RemoveColumns(GridColumns.DependantsCount);
        this.ItemsGridView.RemoveColumnGroups(GridColumnGroups.Education);
        this.ItemsGridView.ChildTableDefinitions.Clear();
      }


      //if (type == EmployeesViewType.Salaries) {

      //}

      OnPropertyChanged(() => ShowCurrent);
      OnPropertyChanged(() => ShowSalary);
      OnPropertyChanged(() => ShowBank);
      OnPropertyChanged(() => ShowEducation);
      OnPropertyChanged(() => ShowDependants);
      OnPropertyChanged(() => CanEnroll);
    }
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
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

    private void OnLoadData(bool reload = false) {
      if (DataInitialized == false || (reload == false && SelectedMonth == CurrentMonth))
        return;

      if (this.SelectedMonth == null && ShowCurrent) {
        CurrentMonth = SelectedMonth;
        OnDataLoaded(new Employee[] { }, null);
        return;
      }

      this.SetBusy();

      if (this.ViewType == EmployeesViewType.Details)
        EmployeesDataManager.GetEmployeeListDetails(SelectedMonth.Value, ListID != null ? ListID.Value.PackArray() : null, true, OnDataLoaded);
      else if (this.ViewType == EmployeesViewType.Salaries)
        EmployeesDataManager.GetEmployeeListSalaries(SelectedMonth.Value, ListID != null ? ListID.Value.PackArray() : null, true, OnDataLoaded);
      else if (this.ViewType == EmployeesViewType.Bank)
        EmployeesDataManager.GetEmployeeListBank(SelectedMonth.Value, ListID != null ? ListID.Value.PackArray() : null, true, OnDataLoaded);
      else if (this.ViewType == EmployeesViewType.Terminations)
        EmployeesDataManager.GetTerminatedEmployees(SelectedMonth, ListID != null ? ListID.Value.PackArray() : null, true, OnDataLoaded);
    }
    private void OnDataLoaded(Employee[] result, Exception error) {
      if (error == null) {
        this.CurrentMonth = this.SelectedMonth;
        AllItems = new ObservableCollection<Employee>(result.OrderBy(s => s.Person.FullName));
        OnFilterData();
        // TODO: Find better way
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

          qry = qry.Where(s => s.Person.FullName.ToLower().Contains(filterText) || s.Dependants.Select(x => x.Person.FullName.ToLower()).Any(y => y.Contains(filterText)));
        }


        Items = new ObservableCollection<Employee>(qry.ToArray());
      } else
        Items = new ObservableCollection<Employee>();


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
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      e.Handled = true;
      if (ShowSalary)
        OnEmployeeEarning();
      else
        OnEditEmployee();
    }

    private void EarningButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEmployeeEarning(); }

    private void OnEmployeeEarning() {
      var wnd = new Earning.EarningWindow((Employee)this.ItemsGridView.SelectedItem, SelectedMonth);
      this.DisplayModal<Earning.EarningWindow>(wnd, OnEarningsWindow);
    }

    private void OnEarningsWindow(Earning.EarningWindow obj) {

    }
    #endregion

    #region Edit
    private EmployeeModificationSection GetModificationSections() {
      EmployeeModificationSection sections = EmployeeModificationSection.Everything;
      if (ShowSalary == false)
        sections &= ~EmployeeModificationSection.Salary;
      if (ShowBank == false)
        sections &= ~EmployeeModificationSection.Bank;
      if (ShowEducation == false)
        sections &= ~EmployeeModificationSection.Education;
      if (CanEnroll == false)
        sections &= ~EmployeeModificationSection.Termination;

      return sections;
    }
    private void EditButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEditEmployee(); }

    private void OnEditEmployee() {
      EmployeeModifyWindow wnd = new EmployeeModifyWindow((Employee)this.ItemsGridView.SelectedItem, GetModificationSections());
      wnd.ListID = ListID;
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }
    #endregion

    #region Add
    private void AddButton_Click(object sender, RoutedEventArgs e) {
      OnNewEmployee();
    }

    private void OnNewEmployee() {
      EmployeeModifyWindow wnd = new EmployeeModifyWindow(ListID, GetModificationSections());
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((EmployeeModifyWindow)sender);
      if (wnd.Result == OperationResult.Add) {
        ReplaceItem(null, wnd.Item);
      } else if (wnd.Result == OperationResult.Delete)
        ReplaceItem(wnd.OriginalItem, wnd.Item);
      else if (wnd.Result == OperationResult.Update) {
        ReplaceItem(wnd.OriginalItem, wnd.Item);
      };
    }

    private void ReplaceItem(Employee oldItem, Employee item) {
      if (oldItem != null) {
        var itm = AllItems.SingleOrDefault(a => a.EmployeeID == oldItem.EmployeeID);
        if (itm != null) {
          AllItems.Remove(itm);
          Items.Remove(itm);
        }
      }

      if (item == null)
        return;

      if ((this.ShowCurrent && item.IsMonthEmployee(CurrentMonth.Value)) || (this.ShowCurrent == false && item.IsTerminated)) {
        AllItems.Add(item);
        this.OnFilterData();
        this.ItemsGridView.SelectItem(item);
      }
    }
    #endregion

    #region Dependant
    private void DeleteDependantButton_Click(object sender, RoutedEventArgs e) {
      var dep = e.SelectParentRow<EmployeeDependant>();

      OnDeleteDependant(dep);
    }

    private void OnDeleteDependant(EmployeeDependant dep) {
      Popup.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDependant (dep, OnDependantDeleted);
      });
    }

    private void OnDependantDeleted(EmployeeDependant result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        result.Employee.Dependants.Remove(result);
        this.ItemsGridView.Rebind();
      } else
        Popup.AlertError(error);
    }

    private void AddDependantButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow<Employee>();
      OnAddDependant();
    }

    private void OnAddDependant() {
      EmployeeDependantModifyWindow wnd = new EmployeeDependantModifyWindow((Employee)this.ItemsGridView.SelectedItem);
      this.DisplayModal<EmployeeDependantModifyWindow>(wnd, OnEmployeeDependantModifyWindowClosed);


    }
    private void EditDependantButton_Click(object sender, RoutedEventArgs e) {
      var dep = e.SelectParentRow<EmployeeDependant>();

      OnEditDependant(dep);
    }

    private void OnEditDependant(EmployeeDependant dep) {
      EmployeeDependantModifyWindow wnd = new EmployeeDependantModifyWindow(dep);
      this.DisplayModal<EmployeeDependantModifyWindow>(wnd, OnEmployeeDependantModifyWindowClosed);
    }

    private void OnEmployeeDependantModifyWindowClosed(EmployeeDependantModifyWindow obj) {
      if (obj.Result == OperationResult.Add) {
        obj.Employee.Dependants.Add(obj.Item);
        this.ItemsGridView.RowFromItem(obj.Employee).IsExpanded = true; 
      } else if (obj.Result == OperationResult.Update) {
        obj.Employee.Dependants.Remove(obj.OriginalItem);
        obj.Employee.Dependants.Add(obj.Item);
      } else if (obj.Result == OperationResult.Delete) {
        obj.Employee.Dependants.Remove(obj.OriginalItem);
      }

      this.ItemsGridView.Rebind();
    }

    #endregion

    #region Documents
    private void DocumentsButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow<Employee>();
      OnEmployeeDocuments();

    }

    private void OnEmployeeDocuments() {
      Documents.DocumentsWindow wnd = new Documents.DocumentsWindow((Employee)this.ItemsGridView.SelectedItem);
      this.DisplayModal(wnd);
    }
    #endregion


    #region Employees ContextMenu
    private void EmployeesContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      if (row != null && row.Item is EmployeeDependant) {
        e.Handled = true;
        return;
      }

      row?.FocusSelect();

      menu.FindMenuItem(MenuItems.NewEmployeeMenuItem).Visibility = menu.FindMenuItem(MenuItems.NewEmployeeSeparatorMenuItem).Visibility = CanEnroll ? Visibility.Visible : Visibility.Collapsed;
      menu.FindMenuItem(MenuItems.AddDependantMenuItem).Visibility = ShowDependants ? Visibility.Visible : Visibility.Collapsed;
      menu.FindMenuItem(MenuItems.EmployeeEarningMenuItem).Visibility = ShowSalary ? Visibility.Visible : Visibility.Collapsed;

      menu.FindMenuItem(MenuItems.EditEmployeeMenuItem).IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem(MenuItems.AddDependantMenuItem).IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem(MenuItems.EmployeeEarningMenuItem).IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem(MenuItems.EmployeeDocumentsMenuItem).IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(); }
    private void EditEmployeeMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnEditEmployee(); }
    private void AddDependantMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnAddDependant(); }
    private void NewEmployeeMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnNewEmployee(); }
    private void EmployeeEarningMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnEmployeeEarning(); }
    private void EmployeeDocumentsMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnEmployeeDocuments(); }
    #endregion

    #region Dependants ContextMenu
    private void DependantsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();


      if (row != null && row.Item is Employee) {
        e.Handled = true;
        return;
      }


      row?.FocusSelect();

      menu.FindMenuItem(MenuItems.EditDependantMenuItem).IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem(MenuItems.DeleteDependantMenuItem).IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void EditDependantMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditDependant((EmployeeDependant)e.SourceOfType<RadMenuItem>().GetClickedRow().Item);
    }

    private void DeleteDependantMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteDependant((EmployeeDependant)e.SourceOfType<RadMenuItem>().GetClickedRow().Item);
    }

    #endregion

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      (sender as FrameworkElement).OpenContextMenu();
    }

    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {
      if (ShowSalary)
        this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Employment", "Position", "Salary");
      else if (ShowBank)
        this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Employment", "Position", "Bank");
      else
        this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Employment", "Position");
    }

    private void FilterListedMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.ItemsGridView.Columns["List"].ColumnFilterDescriptor.SuspendNotifications();
      this.ItemsGridView.Columns["List"].ColumnFilterDescriptor.Clear();
      this.ItemsGridView.Columns["List"].ColumnFilterDescriptor.DistinctFilter.AddDistinctValue(null);
      this.ItemsGridView.Columns["List"].ColumnFilterDescriptor.ResumeNotifications();
    }

    private void ClearFiltersMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.ItemsGridView.FilterDescriptors.Clear();
    }

  }
}
