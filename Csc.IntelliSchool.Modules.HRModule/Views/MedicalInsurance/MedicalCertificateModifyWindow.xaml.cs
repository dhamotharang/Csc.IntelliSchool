using Csc.Components.Common;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using Csc.Wpf.Data;
using System;
using System.Linq;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalCertificateModifyWindow : Csc.Wpf.WindowBase {
    #region Fields
    private EmployeeMedicalCertificate _item;
    private Employee _employee;
    private EmployeeDependant _dependant;
    #endregion

    #region Properties
    public EmployeeMedicalCertificate Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeMedicalCertificate OriginalItem { get; set; }


    public Employee Employee { get { return _employee; } set { if (_employee != value) { _employee = value; OnPropertyChanged(() => Employee); } } }
    public EmployeeDependant Dependant { get { return _dependant; } set { if (_dependant != value) { _dependant = value; OnPropertyChanged(() => Dependant); } } }
    #endregion


    // Constructors
    public MedicalCertificateModifyWindow() {
      InitializeComponent();
    }
    public MedicalCertificateModifyWindow(Employee itm)
      : this() {
      Employee = itm;
      if (itm.MedicalCertificate != null) {
        OriginalItem = itm.MedicalCertificate;
        Item = itm.MedicalCertificate.Clone(false);
      } else {
        Item = new EmployeeMedicalCertificate() {
          Code = itm.EmployeeID.ToString(),
          CertificateOwner = EmployeeMedicalCertificateOwner.Employee,
          CertificateRateType = EmployeeMedicalCertificateOwner.Employee,
        };
      }

      Item.Employees.Clear();
      Item.Employees.Add(itm);
    }
    public MedicalCertificateModifyWindow(EmployeeDependant itm)
      : this() {
      Dependant = itm;
      Employee = itm.Employee;

      if (itm.MedicalCertificate != null) {
        OriginalItem = itm.MedicalCertificate;
        Item = itm.MedicalCertificate.Clone(false);
      } else {
        Item = new EmployeeMedicalCertificate() {
          Code = itm.Employee.MedicalCertificate.Code.ToString(),
          CertificateOwner = EmployeeMedicalCertificateOwner.Dependant,
          CertificateRateType = EmployeeMedicalCertificateOwner.Dependant,
        };
      }
      Item.Dependants.Clear();
      Item.Dependants.Add(itm);
      //Item.Dependant.Employee = itm.Employee;
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.CertificateID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.TypeTextBox.Text = Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee ?
        string.Format(Csc.IntelliSchool.Assets.Resources.HumanResources.Medical_TypeMember, Employee.EmployeeID) :
        string.Format(Csc.IntelliSchool.Assets.Resources.HumanResources.Medical_TypeDependant, Dependant.Employee.Person.FullName, Dependant.EmployeeID);
      this.HireDateTextBox.Text =
        string.Format("{0:d} ({1} year(s), {2} month(s)){3}",
        Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee ? Employee.HireDate : Dependant.Employee.HireDate,
        Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee ? Employee.HireYears : Dependant.Employee.HireYears,
        Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee ? Employee.HireMonths : Dependant.Employee.HireMonths,
        Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee ? "" : ", Employee");

      this.SetBusy();
      EmployeesDataManager.GetMedicalPrograms(false, (res, err) => {
        if (Item.CertificateOwner == EmployeeMedicalCertificateOwner.Dependant && Dependant.Employee != null && Dependant.Employee.MedicalCertificate != null)
          res = res.Where(s => Dependant.Employee.MedicalCertificate.ProgramID == s.ProgramID).ToArray();
        this.ProgramComboBox.FillAsyncItems(res, err, null, this);
        if (Item.CertificateID == 0) {
          this.ProgramComboBox.SelectedValue = Item.ProgramID;
          this.Rebind();
          OnRecalculate();
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
        EmployeesDataManager.DeleteMedicalCertificate(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeMedicalCertificate result, Exception error) {
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

      if (Item.CancellationDate == null)
        Item.CancellationReason = null;

      Item.TrimStrings();


      this.SetBusy();
      OnAddOrUpdate();

      //this.SetBusy();
      //if (string.IsNullOrEmpty(Item.Code)) {
      //  this.SetBusy();
      //  OnAddOrUpdate();
      //} else {
      //  this.SetBusy();
      //  EmployeesDataManager.CheckEmployeeMedicalCodeUsed(Item, OnCheckCompleted);
      //}
    }

    private void OnAddOrUpdate() {
      if (Item.CertificateID == 0)
        EmployeesDataManager.AddOrUpdateMedicalCertificate(Item, OnAdded);
      else
        EmployeesDataManager.AddOrUpdateMedicalCertificate(Item, OnUpdated);
    }

    private void OnCheckCompleted(bool result, Exception error) {
      if (error == null) {
        if (false == result) {
          OnAddOrUpdate();
        } else { // found
          this.ClearBusy();
          this.AlertError(Csc.IntelliSchool.Assets.Resources.HumanResources.Medical_ExistingCode);
        }
      } else {
        this.ClearBusy();
        this.AlertError(error);
      }
    }

    private void OnAdded(EmployeeMedicalCertificate result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeMedicalCertificate result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion


    #region Changes
    private void RecalculateButton_Click(object sender, RoutedEventArgs e) {
      OnRecalculate();
    }

    private void ProgramComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (this.ProgramComboBox.IsDropDownOpen) {// Initialized by User
        this.ProgramComboBox.IsDropDownOpen = false;
        OnRecalculate();
      }
    }

    private void OnRecalculate() {
      Item.Recalculate(this.ProgramComboBox.SelectedItem as EmployeeMedicalProgram, true, true, false);
      this.Rebind();
    }


    private void ConcessionNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      Item.CalculateMonthly();
      Rebind();
    }

    private void MonthlyNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      this.YearlyNumericUpDown.Rebind();
    }


    private void CodeEmployeeIDButton_Click(object sender, RoutedEventArgs e) {
      if (Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee)
        Item.Code = Employee.EmployeeID.ToString();
      else
        Item.Code = Dependant.EmployeeID.ToString();

      this.CodeTextBox.Rebind();
    }

    private void TypeRadioButton_Click(object sender, RoutedEventArgs e) {
      if (this.TypeEmployeeRadioButton.IsChecked == true)
        Item.CertificateRateType = EmployeeMedicalCertificateOwner.Employee;
      else if (this.TypeDependantRadioButton.IsChecked == true)
        Item.CertificateRateType = EmployeeMedicalCertificateOwner.Dependant;
      OnRecalculate();
    }

    private void CustomMonthlyButton_Click(object sender, RoutedEventArgs e) {
      if (this.CustomMonthlyButton.IsChecked == true) {
        Item.Concession = null;
        Rebind();
      } else {
        OnRecalculate();
      }
    }
    #endregion

    private void CodeParentCodeButton_Click(object sender, RoutedEventArgs e) {
      if (Item.CertificateOwner == EmployeeMedicalCertificateOwner.Employee)
        Item.Code = Employee.EmployeeID.ToString();
      else
        Item.Code = Dependant.Employee.MedicalCertificate.Code ?? string.Empty;

      this.CodeTextBox.Rebind();
    }

  }
}