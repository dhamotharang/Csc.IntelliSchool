using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalClaimModifyWindow : Csc.Wpf.WindowBase {
    #region Fields
    private EmployeeMedicalClaim _item;
    private Employee _employee;
    private EmployeeDependant _dependant;
    #endregion

    #region Propreties
    public EmployeeMedicalClaim Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeMedicalClaim OriginalItem { get; set; }

    public Employee Employee { get { return _employee; } set { if (_employee != value) { _employee = value; OnPropertyChanged(() => Employee); } } }
    public EmployeeDependant Dependant { get { return _dependant; } set { if (_dependant != value) { _dependant = value; OnPropertyChanged(() => Dependant); } } }
    #endregion

    // Constructors
    public MedicalClaimModifyWindow() {
      InitializeComponent();
    }
    public MedicalClaimModifyWindow(Employee emp, EmployeeDependant dep)
      : this() {
      Employee = emp;
      Dependant = dep;
      Item = new EmployeeMedicalClaim() { Date = DateTime.Today, EmployeeID = emp.EmployeeID };
      if (dep != null)
        Item.DependantID = dep.DependantID;
    }
    public MedicalClaimModifyWindow(EmployeeMedicalClaim itm)
      : this() {
      Employee = itm.Employee;
      Dependant = itm.Dependant;

      OriginalItem = itm;
      Item = itm.Clone(false);
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.ClaimID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

      this.SetBusy();
      this.SetBusy();
      EmployeesDataManager.GetMedicalClaimTypes(false, (res, err) => {
        this.TypeComboBox.FillAsyncItems(res, err, null, this);
      });
      EmployeesDataManager.GetMedicalClaimStatuses(false, (res, err) => {
        this.StatusComboBox.FillAsyncItems(res, err, a=>a.Name = string.Empty, this);
        if (err == null && Item.ClaimID == 0) {
          var pending = res.Where(s => s.IsPending == true).FirstOrDefault();
          Item.StatusID = pending.StatusID;
          this.StatusComboBox.Rebind();
        }
      });
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteMedicalClaim(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeMedicalClaim result, Exception error) {
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
      OnAddOrUpdate();
    }

    private void OnAddOrUpdate() {
      if (Item.ClaimID == 0)
        EmployeesDataManager.AddOrUpdateMedicalClaim(Item, OnAdded);
      else
        EmployeesDataManager.AddOrUpdateMedicalClaim(Item, OnUpdated);
    }

    private void OnAdded(EmployeeMedicalClaim result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeMedicalClaim result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    private void ClaimAmountNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      this.ClaimedPercentTextBlock.Rebind();
    }

    private void StatusComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      var status = StatusComboBox.SelectedItem as EmployeeMedicalClaimStatus;
      if (status != null && status.IsCompletion && Item.CompletionDate == null) {
        Item.CompletionDate = DateTime.Today;
        this.CompletionDatePicker.Rebind();
      }
    }
  }
}