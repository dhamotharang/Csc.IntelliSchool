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
using Csc.IntelliSchool.Modules.HRModule.Views.Structure;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class ShiftOverridesPage : Csc.Wpf.PageBase {
    #region Fields
    private EmployeeShift[] _shifts;
    private EmployeeShiftOverrideType[] _types;
    private ObservableCollection<EmployeeShiftOverride> _items;
    private Telerik.Windows.Data.ISortDescriptor[] _sortDescriptors;
    #endregion

    #region Properties
    public EmployeeShift[] Shifts { get { return _shifts; } set { _shifts = value; OnPropertyChanged(() => Shifts); } }
    public ObservableCollection<EmployeeShiftOverride> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public EmployeeShiftOverrideType[] Types { get { return _types; } set { if (_types != value) { _types = value; OnPropertyChanged(() => Types); } } }

    // Current loaded details
    public int? CurrentYear { get; set; }
    public EmployeeShift CurrentShift { get; set; }
    public int[] CurrentTypeIDs { get; set; }

    // Selected Items
    public EmployeeShift SelectedShift { get { return this.ShiftComboBox.SelectedItem as EmployeeShift; } set { this.ShiftComboBox.SelectedItem = value; } }
    public int? SelectedYear {
      get {
        var date = this.YearDatePicker.SelectedDate;
        if (date == null)
          return null;

        return date.ToYear().Value.Year;
      }
      set {
        if (value == null)
          this.YearDatePicker.SelectedDate = null;
        else
          this.YearDatePicker.SelectedDate = new DateTime(value.Value, 1, 1);
      }
    }
    public int[] SelectedTypeIDs {
      get {
        return this.TypeComboBox.GetSelectedItemIDs<EmployeeShiftOverrideType>(a => a.TypeID);
      }
      set {
        this.TypeComboBox.SetSelectedItemIDs<EmployeeShiftOverrideType>(value ?? new int[] { }, a => a.TypeID);
      }
    }
    #endregion

    // Constructors
    public ShiftOverridesPage() {
      InitializeComponent();
      this.YearDatePicker.SetPickerTypeYear();
      SelectedYear = CurrentYear = DateTime.Today.Year;
      _sortDescriptors = this.ItemsGridView.SortDescriptors.ToArray();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) { OnLoadFilterData(); }
    private void LoadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(false); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(true); }

    private void OnLoadFilterData() {
      OnLoadShifts();
      OnLoadTypes();
    }

    private void OnLoadShifts() {
      this.SetBusy();
      EmployeesDataManager.GetShifts(OnLoadShiftsCompleted);
    }

    private void OnLoadTypes() {
      this.SetBusy();
      EmployeesDataManager.GetShiftOverrideTypes(OnLoadTypesCompleted);
    }

    private void OnLoadShiftsCompleted(EmployeeShift[] result, Exception error) {
      if (error == null) {
        Shifts = result.OrderBy(s => s.Name).ToArray();
        if (Shifts.Count() == 1) {
          SelectedShift = CurrentShift = Shifts.First();
        }
        OnLoadData(false);
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OnLoadTypesCompleted(EmployeeShiftOverrideType[] result, Exception error) {
      if (error == null) {
        Types = result.InsertNullItem(a => a.Name = Csc.IntelliSchool.Assets.Resources.Text.Text_All).ToArray();
        this.TypeComboBox.SelectedIndex = 0;
        CurrentTypeIDs = SelectedTypeIDs;
        OnLoadData(false);
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OnLoadData(bool reload) {
      if (reload) {
        SelectedYear = CurrentYear;
        SelectedShift = CurrentShift;
        SelectedTypeIDs = CurrentTypeIDs;
      }

      if (SelectedShift == null)
        return;

      this.SetBusy();
      EmployeesDataManager.GetShiftOverrides(SelectedShift.ShiftID, SelectedTypeIDs, SelectedYear, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeShiftOverride[] result, Exception error) {
      if (error == null) {
        this.CurrentShift = SelectedShift;
        this.CurrentYear = SelectedYear;

        Items = new ObservableCollection<EmployeeShiftOverride>(result);
      } else
        Popup.AlertError(error);

      this.ClearBusy();
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
      var itm = (EmployeeShiftOverride)this.ItemsGridView.SelectedItem;
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        EmployeesDataManager.DeleteShiftOverride(itm, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeShiftOverride result, Exception error) {
      if (error == null) {
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
      ShiftOverrideModifyWindow wnd = new ShiftOverrideModifyWindow(this.CurrentShift);
      this.DisplayModal<ShiftOverrideModifyWindow>(wnd, OnModifyWindowClosed);
    }


    private void OnModifyWindowClosed(ShiftOverrideModifyWindow wnd) {
      if (wnd.Result == OperationResult.None)
        return;

      DateTime? yearStartDate = null, yearEndDate = null;

      if (CurrentYear != null) {
        yearStartDate = new DateTime(CurrentYear.Value, 1, 1);
        yearEndDate = yearStartDate.ToYearEnd();
      }

      bool validItem = (CurrentShift != null && wnd.Item.ShiftID == CurrentShift.ShiftID) &&
        (CurrentTypeIDs == null || CurrentTypeIDs.Contains(0) || (CurrentTypeIDs != null && wnd.Item.TypeID != null && CurrentTypeIDs.Contains(wnd.Item.TypeID.Value))) &&
        (CurrentYear == null || (DateTimeExtensions.Overlaps(wnd.Item.StartDate, wnd.Item.EndDate, yearStartDate.Value, yearEndDate.Value)));

      if (wnd.Result == OperationResult.Add && validItem) {
        Items.Add(wnd.Item);
        this.ItemsGridView.SelectItem(wnd.Item);
      } else if (wnd.Result == OperationResult.Update) {
        Items.Remove(wnd.OriginalItem);
        if (validItem) {
          Items.Add(wnd.Item);
          this.ItemsGridView.SelectItem(wnd.Item);
        }
      } else if (wnd.Result == OperationResult.Delete) {
        Items.Remove(wnd.OriginalItem);
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
      var itm = (EmployeeShiftOverride)this.ItemsGridView.SelectedItem;
      ShiftOverrideModifyWindow wnd = new ShiftOverrideModifyWindow(itm);
      this.DisplayModal<ShiftOverrideModifyWindow>(wnd, OnModifyWindowClosed);
    }


    #endregion

    #region Extras
    private void ApplyOrderMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      using (var d = Dispatcher.DisableProcessing()) {
        this.ItemsGridView.SortDescriptors.Clear();
        this.ItemsGridView.SortDescriptors.AddRange(_sortDescriptors);
      }
    }
    #endregion

  }
}
