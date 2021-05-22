using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeSelectionControl : Csc.Wpf.UserControlBase {
    #region Events
    public event EventHandler ItemSelected;
    #endregion

    #region Fields
    private Employee _selectedEmployee;
    //private bool _needsSorting = true;
    private Employee[] _items;
    private EmployeeSelectionType _selectionType;
    private EmployeeDependant _selectedDependant;
    #endregion

    #region Propreties
    public Employee[] AllItems { get; set; }
    public Employee[] Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public int? ParamListID { get; set; }
    public bool IsMedical { get { return SelectionType == EmployeeSelectionType.Medical; } }
    public EmployeeSelectionType SelectionType
    {
      get { return _selectionType; }
      set
      {
        if (_selectionType != value) {
          _selectionType = value;

          SetLayout();
          OnPropertyChanged(() => SelectionType);
          OnPropertyChanged(() => IsMedical);
        }
      }
    }

    public DateTime? SelectedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } set { this.MonthDatePicker.SelectedDate = value.ToMonth(); } }
    public DateTime? CurrentMonth { get; set; }
    public override bool DataInitialized
    {
      get
      {
        return this.ListsComboBox.ItemsSource != null &&
          this.BranchComboBox.ItemsSource != null &&
          this.DepartmentComboBox.ItemsSource != null &&
          this.PositionComboBox.ItemsSource != null;
      }
    }
    public Employee SelectedEmployee { get { return _selectedEmployee; } set { if (_selectedEmployee != value) { _selectedEmployee = value; OnPropertyChanged(() => SelectedEmployee); } } }
    public EmployeeDependant SelectedDependant { get { return _selectedDependant; } set { if (_selectedDependant != value) { _selectedDependant = value; OnPropertyChanged(() => SelectedDependant); } } }
    #endregion

    public EmployeeSelectionControl() {
      InitializeComponent();
      this.MonthDatePicker.SetPickerTypeMonth();
      this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
    }

    #region Loading
    private void SetLayout() {
      this.MonthDatePicker.IsEnabled = this.IsMedical == false;
      //foreach(var col in this.ItemsGridView.Columns) {
      //  if (col.ColumnGroupName != null && col.ColumnGroupName.ToString() == "Medical")
      //    col.IsVisible = this.IsMedical;
      //}
    }


    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) {
      var watch = CountDownWatch.Lock(4, OnFilterDataLoaded);

      OnGetBranches(watch);
      OnGetDepartments(watch);
      OnGetPositions(watch);
      OnGetLists(watch);
    }

    private void OnGetBranches(CountDownWatch watch) {
      this.SetBusy();
      EmployeesDataManager.GetBranches(false, (res, err) => {
        this.BranchComboBox.FillAsyncItems(res, err, (itm) => itm.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All, this);
        watch.Release();
      });
    }

    private void OnGetDepartments(CountDownWatch watch) {
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, ParamListID, (res, err) => {
        this.DepartmentComboBox.FillAsyncItems(res, err, (itm) => itm.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All, this);
        watch.Release();
      });
    }

    private void OnGetPositions(CountDownWatch watch) {
      this.SetBusy();
      EmployeesDataManager.GetPositions(false, ParamListID, (res, err) => {
        this.PositionComboBox.FillAsyncItems(res, err, (itm) => itm.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All, this);
        watch.Release();
      });
    }

    private void OnGetLists(CountDownWatch watch) {
      this.SetBusy();
      EmployeesDataManager.GetLists((res, err) => {
        this.ListsComboBox.FillAsyncItems(res, err, (itm) => itm.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_Unlisted, this);
        this.ListsComboBox.SelectAll();
        watch.Release();
      });
    }

    private void OnFilterDataLoaded(object sender, EventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void OnLoadData(bool reload = false) {
      if (DataInitialized == false || (SelectionType != EmployeeSelectionType.Medical && reload == false && SelectedMonth == CurrentMonth))
        return;

      if (this.SelectedMonth == null && SelectionType == EmployeeSelectionType.Default) {
        CurrentMonth = SelectedMonth;
        OnDataLoaded(new Employee[] { }, null);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.SelectEmployees(SelectionType, SelectedMonth, ParamListID != null ? ParamListID.Value.PackArray() : null, OnDataLoaded);
    }

    private void OnDataLoaded(Employee[] result, Exception error) {
      if (result != null) {
        CurrentMonth = SelectedMonth;
        AllItems = result;
        OnFilterData();
      } else {
        Popup.AlertError(error);
      }
      this.ClearBusy();
    }

    private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { OnFilterData(); }
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }
    private void MonthDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) { OnLoadData(); }
    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) { if (e.Key == System.Windows.Input.Key.Enter) { OnFilterData(); } }
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

        if (this.BranchComboBox.SelectedID() > 0)
          qry = qry.Where(s => s.BranchID == this.BranchComboBox.SelectedID().Value);
        if (this.DepartmentComboBox.SelectedID() > 0)
          qry = qry.Where(s => s.DepartmentID == this.DepartmentComboBox.SelectedID().Value);
        if (this.PositionComboBox.SelectedID() > 0)
          qry = qry.Where(s => s.PositionID == this.PositionComboBox.SelectedID().Value);
        if (this.NameTextBox.Text.Length > 0) {
          string filterText = this.NameTextBox.Text.ToLower();

          //qry = qry.Where(s => s.FullName.ToLower().Contains(filterText) || s.Dependants.Select(x => x.FullName.ToLower()).Any(y => y.Contains(filterText)));
          qry = qry.Where(s => s.Person.FullName.ToLower().Contains(filterText));
        }

        Items = qry.ToArray();
      } else
        Items = new Employee[] { };

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("Name");
      //}
      //_needsSorting = Items.Count() == 0;
    }
    #endregion

    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      SelectedEmployee = (Employee)e.Row.Item;
      OnSelected();
    }

    private void DependantsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      SelectedDependant = (EmployeeDependant)e.Row.Item;
      SelectedEmployee = SelectedDependant.Employee;
      OnSelected();
    }

    private void OnSelected() {
      ItemSelected?.Invoke(this, EventArgs.Empty);
    }


    #region Menu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();

    }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(true); }

    #endregion

  }
}
