using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Windows;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class LoanModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeLoanProxy _item;
    private Employee _employee;

    // Properties
    public EmployeeLoanProxy Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeLoan OriginalItem { get; set; }
    public EmployeeLoan UpdatedItem { get; set; }
    public Employee Employee { get { return _employee; } set { if (_employee != value) { _employee = value; OnPropertyChanged(() => Employee); } } }


    // Constructors
    public LoanModifyWindow() {
      InitializeComponent();
      this.StartMonthPicker.SetPickerTypeMonth();
      this.EndMonthPicker.SetPickerTypeMonth();
    }
    public LoanModifyWindow(Employee emp, DateTime? month) : this() {
      Item = EmployeeLoanProxy.CreateObject(month);
      Employee = emp;
      Item.EmployeeID = emp.EmployeeID;
    }
    public LoanModifyWindow(Employee emp, EmployeeLoan item)
      : this() {
      Employee = emp;
      if (item != null) {
        OriginalItem = item;
        Item = EmployeeLoanProxy.CreateObject(item);
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.NewItem ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.NewItem ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    private void SingleButton_Click(object sender, RoutedEventArgs e) {
      Item.EndMonth = Item.StartMonth;
    }
    private void StartMonthPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      SetInstallment();
    }
    private void EndMonthPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      SetInstallment();
    }
    private void TotalAmountNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      SetInstallment();
    }

    private void SetInstallment() {
      if (Item.TotalAmount == null || Item.StartMonth == null || Item.EndMonth == null || Item.StartMonth > Item.EndMonth) {
        Item.Installment = null;
        return;
      }

      var monthCount = 1 + DateTimeExtensions.CalculatePeriod(Item.StartMonth.Value, Item.EndMonth.Value).Months;
      Item.Installment = (int)Math.Ceiling((decimal)Item.TotalAmount / (decimal)monthCount);
    }

    private void InstallmentNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (Item.TotalAmount == null || Item.TotalAmount == 0 || Item.StartMonth == null || Item.Installment == null || Item.Installment == 0) {
        return;
      }

      var monthCount = (int)Math.Ceiling(((decimal)Item.TotalAmount / (decimal)Item.Installment)) - 1;
      Item.EndMonth = Item.StartMonth.Value.AddMonths(monthCount);
    }

    private void PreviewButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false) {
        this.InstallmentsGridView.ItemsSource = new EmployeeLoanInstallment[] { };
        return;
      }

      this.InstallmentsGridView.ItemsSource = EmployeeLoanInstallment.Generate(Item);
    }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteLoan(EmployeeLoan.CreateObject(Item), OnDeleted);
      });
    }

    private void OnDeleted(EmployeeLoan result, Exception error) {
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
      if (Item.NewItem)
        EmployeesDataManager.AddOrUpdateLoan(Item, OnAdded);
      else
        EmployeesDataManager.AddOrUpdateLoan(Item, OnUpdated);
    }

    private void OnAdded(EmployeeLoan result, Exception error) {
      if (error == null) {
        result.Employee = Employee;
        UpdatedItem = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeLoan result, Exception error) {
      if (error == null) {
        result.Employee = Employee;
        UpdatedItem = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion


  }

}