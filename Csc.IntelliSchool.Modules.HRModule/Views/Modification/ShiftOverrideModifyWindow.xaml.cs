using Csc.Components.Common;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using Csc.Wpf.Data;
using System;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class ShiftOverrideModifyWindow : Csc.Wpf.WindowBase {
    #region Fields
    private EmployeeShift[] _shifts;
    private EmployeeShiftOverride _item;
    private RadTimePicker[] _timePickers;
    #endregion

    #region Properties
    public EmployeeShift[] Shifts { get { return _shifts; } set { if (_shifts != value) { _shifts = value; OnPropertyChanged(() => Shifts); } } }
    public EmployeeShiftOverride Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeShiftOverride OriginalItem { get; set; }
    public EmployeeShift SelectedShift { get { return this.ShiftComboBox.SelectedItem as EmployeeShift; } }
    #endregion


    // Constructors
    public ShiftOverrideModifyWindow(EmployeeShift shift = null) {
      Initialize();
      Item = EmployeeShiftOverride.CreateObject(shift);
    }
    public ShiftOverrideModifyWindow(EmployeeShiftOverride item) {
      Initialize();
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone(false);
      }
    }


    #region Inti
    private void Initialize() {
      InitializeComponent();
      LoadTimePickers();
      this.FromMarginTimePicker.FormatTimeSpan();
      this.ToMarginTimePicker.FormatTimeSpan();
    }
    private void LoadTimePickers() {
      _timePickers = new RadTimePicker[] {
        this.SaturdaysFromTimePicker,
        this.SaturdaysToTimePicker,

        this.SundaysFromTimePicker,
        this.SundaysToTimePicker,

        this.MondaysFromTimePicker,
        this.MondaysToTimePicker,

        this.TuesdaysFromTimePicker,
        this.TuesdaysToTimePicker,

        this.WednesdaysFromTimePicker,
        this.WednesdaysToTimePicker,

        this.ThursdaysFromTimePicker,
        this.ThursdaysToTimePicker,

        this.FridaysFromTimePicker,
        this.FridaysToTimePicker,
        };
    }

    #endregion


    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {


      this.DeleteButton.Visibility = Item.OverrideID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.OverrideID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.ShiftComboBox.Focus();
      //this.NameTextBox.Focus();

      OnLoadShifts();
      OnLoadTypes();
    }


    private void OnLoadShifts() {
      this.SetBusy();
      EmployeesDataManager.GetShifts((res, err) => {
        if (err == null) {
          this.Shifts = res;
        } else {
          this.AlertError(err);
        }

        this.ClearBusy();
      });
    }

    private void OnLoadTypes(EmployeeShiftOverrideType itemToSelect = null) {
      this.SetBusy();
      EmployeesDataManager.GetShiftOverrideTypes((res, err) => {
        this.TypeComboBox.FillAsyncItems(res, err, a => a.Name = "",  this);
        if (itemToSelect != null)
          this.TypeComboBox.SetSelectedItemIDs<EmployeeShiftOverrideType>(itemToSelect.TypeID.PackArray(), a => a.TypeID);
      });
    }
    #endregion

    #region Basic
    private void ShiftComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (SelectedShift == null)
        return;

      Item.SaturdaysFrom = SelectedShift.SaturdaysFrom;
      Item.SaturdaysTo = SelectedShift.SaturdaysTo;

      Item.SundaysFrom = SelectedShift.SundaysFrom;
      Item.SundaysTo = SelectedShift.SundaysTo;

      Item.MondaysFrom = SelectedShift.MondaysFrom;
      Item.MondaysTo = SelectedShift.MondaysTo;

      Item.TuesdaysFrom = SelectedShift.TuesdaysFrom;
      Item.TuesdaysTo = SelectedShift.TuesdaysTo;

      Item.WednesdaysFrom = SelectedShift.WednesdaysFrom;
      Item.WednesdaysTo = SelectedShift.WednesdaysTo;

      Item.ThursdaysFrom = SelectedShift.ThursdaysFrom;
      Item.ThursdaysTo = SelectedShift.ThursdaysTo;

      Item.FridaysFrom = SelectedShift.FridaysFrom;
      Item.FridaysTo = SelectedShift.FridaysTo;

      RebindTimePickers();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteShiftOverride(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeShiftOverride result, Exception error) {
      if (error == null) {
        this.Close(OperationResult.Delete);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Saving
    private void SaveButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      if (Item.Validate() == false) {
        this.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Error_Validation);
        return;
      }

      VerifyItem();

      Item.TrimStrings();

      this.SetBusy();
      if (Item.OverrideID == 0)
        EmployeesDataManager.AddOrUpdateShiftOverride(Item, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateShiftOverride(Item, OnUpdated);
      }
    }

    private void VerifyItem() {
      if (SelectedShift.Saturdays == false)
        Item.SaturdaysFrom = Item.SaturdaysTo = null;

      if (SelectedShift.Sundays == false)
        Item.SundaysFrom = Item.SundaysTo = null;

      if (SelectedShift.Mondays == false)
        Item.MondaysFrom = Item.MondaysTo = null;

      if (SelectedShift.Tuesdays == false)
        Item.TuesdaysFrom = Item.TuesdaysTo = null;

      if (SelectedShift.Wednesdays == false)
        Item.WednesdaysFrom = Item.WednesdaysTo = null;

      if (SelectedShift.Thursdays == false)
        Item.ThursdaysFrom = Item.ThursdaysTo = null;

      if (SelectedShift.Fridays == false)
        Item.FridaysFrom = Item.FridaysTo = null;

      OnPropertyChanged(() => Item);
    }

    private void OnAdded(EmployeeShiftOverride result, Exception error) {
      if (error == null) {
        Item = result;
        Item.Shift = SelectedShift;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeShiftOverride result, Exception error) {
      if (error == null) {
        Item = result;
        Item.Shift = SelectedShift;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Types
    private void AddTypeButton_Click(object sender, RoutedEventArgs e) {
      var wnd = new Lookup.ShiftOverrideTypeModifyWindow();
      wnd.Closed += TypeModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void TypeModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadTypes(((Lookup.ShiftOverrideTypeModifyWindow)sender).Item);
    }
    #endregion

    #region Helpers
    private void RebindTimePickers() {
      _timePickers.Rebind();
    }
    #endregion
  }

}