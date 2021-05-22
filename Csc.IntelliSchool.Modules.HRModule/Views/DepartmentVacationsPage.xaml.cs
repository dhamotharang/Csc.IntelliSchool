using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class DepartmentVacationsPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeDepartmentVacation> _items;
    private EmployeeDepartment[] _departments;
    //private bool _needsSorting;
    #endregion

    #region Properties
    public override bool DataInitialized { get { return Departments != null; } }
    public ObservableCollection<EmployeeDepartmentVacation> AllVacations { get; set; }
    public ObservableCollection<EmployeeDepartmentVacation> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public EmployeeDepartment[] Departments { get { return _departments; } set { _departments = value; OnPropertyChanged(() => Departments); } }
    public int? SelectedDepartmentID {
      get {
        if (this.DepartmentComboBox.SelectedValue == null || (int)this.DepartmentComboBox.SelectedValue == 0)
          return null;

        return (int)this.DepartmentComboBox.SelectedValue;
      }
    }
    public int? CurrentYear { get; set; }
    public int? SelectedYear { get { return this.YearDatePicker.SelectedDate != null ? this.YearDatePicker.SelectedDate.Value.Year : new int?(); } }
    #endregion

    // Constructors
    public DepartmentVacationsPage() {
      InitializeComponent();
      //SortDefault();
      this.YearDatePicker.SetPickerTypeYear();
      this.YearDatePicker.SelectedDate = DateTime.Today.ToYear();
    }

    //private void SortDefault() {
    //  this.ItemsGridView.SortBy("StartDate", "Name");
    //}

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadFilterData();
    }
    private void IncludeSummariesButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void YearDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnLoadData(); }

    private void OnLoadFilterData() {
      // one for each call
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, null, OnGetDepartmentsCompleted);
    }
    private void OnGetDepartmentsCompleted(EmployeeDepartment[] result, Exception error) {
      if (error == null) {
        Departments = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OnLoadData(bool reload = false) {
      if (DataInitialized == false || (reload == false && SelectedYear == CurrentYear))
        return;

      if (this.SelectedYear == null) {
        CurrentYear = SelectedYear;
        OnDataLoaded(new EmployeeDepartmentVacation[] { }, null);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.GetDepartmentVacations(SelectedYear.Value, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeDepartmentVacation[] result, Exception error) {
      if (error == null) {
        this.CurrentYear = this.SelectedYear;
        AllVacations = new ObservableCollection<EmployeeDepartmentVacation>(result.OrderBy(s => s.StartDate));
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { OnFilterData(); }
    private void OnFilterData() {
      if (AllVacations == null)
        return;

      var qry = AllVacations.AsQueryable();
      if (SelectedDepartmentID != null)
        qry = qry.Where(s => s.Departments .Where(x=>x.DepartmentID == SelectedDepartmentID).Count()> 0 );

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  SortDefault();

      //}

      Items = new ObservableCollection<EmployeeDepartmentVacation>(qry.ToArray());
      //_needsSorting = Items.Count() == 0;
    }
    #endregion

    #region Edit
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { e.Handled = true; OnEditItem(); }
    private void EditButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEditItem(); }

    private void OnEditItem() {
      DepartmentVacationModifyWindow wnd = new DepartmentVacationModifyWindow((EmployeeDepartmentVacation)this.ItemsGridView.SelectedItem);
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }
    #endregion

    #region Add
    private void AddButton_Click(object sender, RoutedEventArgs e) {
      OnNewItem();
    }

    private void OnNewItem() {
      DepartmentVacationModifyWindow wnd = new DepartmentVacationModifyWindow(SelectedDepartmentID);
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((DepartmentVacationModifyWindow)sender);
      if (wnd.Result == OperationResult.Add) {
        ReplaceItem(null, wnd.Item);
      } else if (wnd.Result == OperationResult.Delete)
        ReplaceItem(wnd.OriginalItem, wnd.Item);
      else if (wnd.Result == OperationResult.Update) {
        ReplaceItem(wnd.OriginalItem, wnd.Item);
      };
    }

    private void ReplaceItem(EmployeeDepartmentVacation oldItem, EmployeeDepartmentVacation newItem) {
      if (oldItem != null) {
        var itm = AllVacations.SingleOrDefault(a => a.VacationID == oldItem.VacationID);
        if (itm != null) {
          AllVacations.Remove(itm);
          Items.Remove(itm);
        }
      }

      if (newItem == null)
        return;

      if (newItem.GetDays().Any(s => s.Year == CurrentYear.Value)) {
        AllVacations.Add(newItem);

        if (SelectedDepartmentID == null || newItem.Departments.Where(s=>s.DepartmentID ==  SelectedDepartmentID ) .Count() > 0) {
          Items.Add(newItem);
          this.ItemsGridView.SelectItem(newItem);
        }
      }
    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnDeleteItem(); }
    private void OnDeleteItem() {
      var itm = (EmployeeDepartmentVacation)this.ItemsGridView.SelectedItem;
      Popup.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDepartmentVacation(itm, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDepartmentVacation result, Exception error) {
      if (error == null) {
        ReplaceItem(result, null);
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


      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(); }
    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnEditItem(); }
    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnDeleteItem(); }
    private void NewMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnNewItem(); }

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      (sender as FrameworkElement).OpenContextMenu();
    }

    #endregion


  }
}
