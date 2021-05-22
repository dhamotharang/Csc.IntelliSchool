using Csc.Components.Common;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class ChargeModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeCharge _item;
    private DateTime? _selectedStartMonth;
    private DateTime? _selectedEndMonth;
    private Employee _employee;

    // Properties
    public EmployeeCharge Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeCharge OriginalItem { get; set; }
    public Employee Employee { get { return _employee; } set { if (_employee != value) { _employee = value; OnPropertyChanged(() => Employee); } } }


    public DateTime? SelectedStartMonth { get { return _selectedStartMonth; } set { _selectedStartMonth = value; OnPropertyChanged(() => SelectedStartMonth); } }
    public DateTime? SelectedEndMonth { get { return _selectedEndMonth; } set { _selectedEndMonth = value; OnPropertyChanged(() => SelectedEndMonth); } }

    // Constructors
    public ChargeModifyWindow() {
      InitializeComponent();

      this.StartMonthPicker.SetPickerTypeMonth();
      this.EndMonthPicker.SetPickerTypeMonth();
    }
    public ChargeModifyWindow(Employee emp)
      : this() {
      Employee = emp;
      Item = EmployeeCharge.CreateObject(emp.EmployeeID);
      this.StartMonthPicker.SelectedValue = null;
      this.EndMonthPicker.SelectedValue = null;
    }
    public ChargeModifyWindow(Employee emp, EmployeeCharge item)
      : this() {
      Employee = emp;
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone(false);
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.ChargeID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.ChargeID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.StartMonthPicker.Focus();

      OnLoadTypes();
    }

    private void OnLoadTypes(EmployeeChargeType itemToSelect = null) {
      this.SetBusy();
      EmployeesDataManager.GetChargeTypes(false, (res, err) => this.TypeComboBox.FillAsyncItems(res, err, a => a.Name = "", itemToSelect, this));
    }
    #endregion

    #region Basic
    private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
 
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    private void SingleToggleButton_Click(object sender, RoutedEventArgs e) {
      if (this.SingleToggleButton.IsChecked == true) {
        Item.EndMonth = Item.StartMonth;
        this.EndMonthPicker.Rebind();
      }
    }
    private void StartMonthPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (this.SingleToggleButton.IsChecked == true) {
        Item.EndMonth = Item.StartMonth;
        this.EndMonthPicker.Rebind();
      }
      this.MonthsTextBlock.Rebind();
    }
    private void EndMonthPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      this.MonthsTextBlock.Rebind();
    }


    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteCharge (Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeCharge result, Exception error) {
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

      Item.TrimStrings();

      if (Item.StartMonth != null)
        Item.StartMonth = Item.StartMonth.ToMonth();
      if (Item.EndMonth != null)
        Item.EndMonth = Item.EndMonth.ToMonth();

      this.SetBusy();
      if (Item.ChargeID == 0)
        EmployeesDataManager.AddOrUpdateCharge(Item, OnAdded);
      else
        EmployeesDataManager.AddOrUpdateCharge(Item, OnUpdated);
    }

    private void OnAdded(EmployeeCharge result, Exception error) {
      if (error == null) {
        result.Employee = Employee;
        result.Type = this.TypeComboBox.SelectedItem as EmployeeChargeType;


        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeCharge result, Exception error) {
      if (error == null) {
        result.Employee = Employee;
        result.Type = this.TypeComboBox.SelectedItem as EmployeeChargeType;

        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Types
    private void AddTypeButton_Click(object sender, RoutedEventArgs e) {
      var wnd = new Lookup.ChargeTypeModifyWindow();
      wnd.Closed += TypeModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void TypeModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadTypes(((Lookup.ChargeTypeModifyWindow)sender).Item);
    }
    #endregion
  }

}