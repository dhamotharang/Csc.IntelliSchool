using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class VacationModifyWindow : Csc.Wpf.WindowBase {
    #region Fields
    private EmployeeVacation _item;
    private DateTime? _selectedStartDate;
    private DateTime? _selectedEndDate;
    private Employee _employee;
    private bool _isTypeDataBindCompleted = false;
    #endregion

    #region Properties
    public EmployeeVacation Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeVacation OriginalItem { get; set; }
    public Employee Employee { get { return _employee; } set { if (_employee != value) { _employee = value; OnPropertyChanged(() => Employee); } } }
    
    public DateTime? SelectedStartDate { get { return _selectedStartDate; } set { _selectedStartDate = value; OnPropertyChanged(() => SelectedStartDate); } }
    public DateTime? SelectedEndDate { get { return _selectedEndDate; } set { _selectedEndDate = value; OnPropertyChanged(() => SelectedEndDate); } }
    #endregion


    // Constructors
    public VacationModifyWindow() {
      InitializeComponent();
    }

    public VacationModifyWindow(Employee emp)
      : this() {
      Employee = emp;
      Item = EmployeeVacation.CreateObject(emp.EmployeeID);
      this.StartDatePicker.SelectedValue = null;
      this.EndDatePicker.SelectedValue = null;

    }
    public VacationModifyWindow(Employee emp, EmployeeVacation item)
      : this() {
      Employee = emp;
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone(false);
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.VacationID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.VacationID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.StartDatePicker.Focus();

      OnLoadTypes();
    }

    private void OnLoadTypes(EmployeeVacationType itemToSelect = null) {
      this.SetBusy();
      EmployeesDataManager.GetVacationTypes(false, (res, err) => this.TypeComboBox.FillAsyncItems(res, err, a => a.Name = "", itemToSelect, this));
    }
    #endregion

    #region Basic
    private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      lock (this) {
        if (_isTypeDataBindCompleted == false) { // first time
          _isTypeDataBindCompleted = true;
          return;
        }
      }

      var type = this.TypeComboBox.SelectedItem as EmployeeVacationType;

      if (type != null && type.TypeID != 0 && type.IsPaid != Item.IsPaid) {
        Item.IsPaid = type.IsPaid;
        this.PaidCheckBox.Rebind();
      }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    private void SingleToggleButton_Click(object sender, RoutedEventArgs e) {
      if (this.SingleToggleButton.IsChecked == true) {
        Item.EndDate = Item.StartDate;
        this.EndDatePicker.Rebind();
        RefreshDayCount();
      }
    }
    private void StartDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (this.SingleToggleButton.IsChecked == true) {
        Item.EndDate = Item.StartDate;
        this.EndDatePicker.Rebind();
      }
      RefreshDayCount();
    }
    private void EndDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      RefreshDayCount();
    }

    private void RefreshDayCount() {
      this.DaysTextBlock.Rebind(System.Windows.Controls.TextBlock.TextProperty);
    }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteVacation (Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeVacation result, Exception error) {
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

      this.SetBusy();
      if (Item.VacationID == 0)
        EmployeesDataManager.AddOrUpdateVacation(Item, OnAdded);
      else
        EmployeesDataManager.AddOrUpdateVacation(Item, OnUpdated);
    }

    private void OnAdded(EmployeeVacation result, Exception error) {
      if (error == null) {
        result.Employee = Employee;
        result.Type = this.TypeComboBox.SelectedItem as EmployeeVacationType;
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeVacation result, Exception error) {
      if (error == null) {
        result.Employee = Employee;
        result.Type = this.TypeComboBox.SelectedItem as EmployeeVacationType;
        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Types
    private void AddTypeButton_Click(object sender, RoutedEventArgs e) {
      var wnd = new Lookup.VacationTypeModifyWindow();
      wnd.Closed += TypeModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void TypeModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadTypes(((Lookup.VacationTypeModifyWindow)sender).Item);
    }
    #endregion
  }

}