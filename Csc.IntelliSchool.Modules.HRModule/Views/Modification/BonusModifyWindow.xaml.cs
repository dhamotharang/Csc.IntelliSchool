using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Windows;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;


namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class BonusModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeBonus _item;
    private Employee _employee;

    // Properties
    public EmployeeBonus Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeBonus OriginalItem { get; set; }
    public Employee Employee { get { return _employee; } set { if (_employee != value) { _employee = value; OnPropertyChanged(() => Employee); } } }


    // Constructors
    public BonusModifyWindow() {
      InitializeComponent();
    }

    public BonusModifyWindow(Employee emp)
      : this() {
      Employee = emp;
      Item = EmployeeBonus.CreateObject(emp.EmployeeID);
    }
    public BonusModifyWindow(Employee emp, EmployeeBonus item)
      : this() {
      Employee = emp;
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone(false);
        this.PointsRadioButton.IsChecked = Item.Points != null;
        this.ValueRadioButton.IsChecked = Item.Value != null;
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.BonusID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.BonusID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.DatePicker.Focus();
      OnLoadTypes();
    }

    private void OnLoadTypes(EmployeeBonusType itemToSelect = null) {
      this.SetBusy();
      EmployeesDataManager.GetBonusTypes(false, (res, err) => this.TypeComboBox.FillAsyncItems(res, err, a => a.Name = "", itemToSelect, this));
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteBonus(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeBonus result, Exception error) {
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

      if (this.PointsRadioButton.IsChecked == true)
        Item.Value = null;
      else
        Item.Points = null;

      this.SetBusy();
      if (Item.BonusID == 0)
        EmployeesDataManager.AddOrUpdateBonus(Item, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateBonus(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeBonus result, Exception error) {
      if (error == null) {
        Item = result;
        Item.Employee = Employee;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeBonus result, Exception error) {
      if (error == null) {
        Item = result;
        Item.Employee = Employee;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Types
    private void AddTypeButton_Click(object sender, RoutedEventArgs e) {
      var wnd = new Lookup.BonusTypeModifyWindow();
      wnd.Closed += TypeModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void TypeModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadTypes(((Lookup.BonusTypeModifyWindow)sender).Item);
    }
    #endregion
  }

}