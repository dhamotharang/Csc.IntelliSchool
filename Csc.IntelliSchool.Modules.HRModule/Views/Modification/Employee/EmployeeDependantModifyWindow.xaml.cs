using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Windows;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeDependantModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeDependant _item;
    private Employee _employee;

    // Properties
    public Employee Employee { get { return _employee; } set { _employee = value; OnPropertyChanged(() => Employee); } }
    public EmployeeDependant Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeDependant OriginalItem { get; set; }

    // Constructors
    public EmployeeDependantModifyWindow(Employee emp) {
      InitializeComponent();
      this.TypeComboBox.FillItems(typeof(EmployeeDependantType), true);
      this.GenderComboBox.FillItems(PeopleDataManager.GenderList, true);

      Employee = emp;
      Item = new EmployeeDependant() { EmployeeID = emp.EmployeeID};
      Item.Person = new Person() { NationalityID = emp.Person.NationalityID, ReligionID = emp.Person.ReligionID };
    }
    public EmployeeDependantModifyWindow(EmployeeDependant item)
      : this(item.Employee) {
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone(false);
        Item.Person = item.Person.Clone(false);
      }
    }


    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.DependantID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.DependantID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.FirstNameTextBox.Focus();

      this.SetBusy();
      PeopleDataManager.GetNationalities(false, (res, err) => this.NationalityComboBox.FillAsyncItems(res, err, a => a.Name = "", this));
    }
    #endregion

    #region Basic
    private void BirthdateDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      this.BirthdateDatePicker.SetNullIfEmpty();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDependant (Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDependant result, Exception error) {
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
      Item.Person.FirstName = Item.Person.FirstName.ToTitleCase();
      Item.Person.MiddleName = Item.Person.MiddleName.ToTitleCase();
      Item.Person.LastName = Item.Person.LastName.ToTitleCase();
      Item.Person.FamilyName = Item.Person.FamilyName.ToTitleCase();

      this.SetBusy();
      if (Item.DependantID == 0)
        EmployeesDataManager.AddOrUpdateDependant(Item, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateDependant(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeDependant result, Exception error) {
      if (error == null) {
        Item = result;
        Item.Employee = Employee;
        Item.EmployeeID = Employee.EmployeeID;
        if (OriginalItem != null)
          Item.MedicalCertificate = OriginalItem.MedicalCertificate;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeDependant result, Exception error) {
      if (error == null) {
        Item = result;
        Item.Employee = Employee;
        Item.EmployeeID = Employee.EmployeeID;
        if (OriginalItem != null)
          Item.MedicalCertificate = OriginalItem.MedicalCertificate;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (Item.DependantType == EmployeeDependantType.Child && Employee.Person.GenderTyped == Gender.Male) {
        Item.Person.MiddleName = Employee.Person.FirstName;
        Item.Person.LastName = Employee.Person.MiddleName ?? string.Empty;
        Item.Person.FamilyName = string.IsNullOrEmpty(Employee.Person.FamilyName) == false ? Employee.Person.FamilyName : Employee.Person.LastName;

        Item.Person.ArabicMiddleName = Employee.Person.ArabicMiddleName;
        Item.Person.ArabicLastName = Employee.Person.ArabicLastName ?? string.Empty;
        Item.Person.ArabicFamilyName = string.IsNullOrEmpty(Employee.Person.ArabicFamilyName) == false ? Employee.Person.ArabicFamilyName : Employee.Person.ArabicLastName;

        this.MiddleNameTextBox.Rebind();
        this.LastNameTextBox.Rebind();
        this.FamilyNameTextBox.Rebind();
        this.ArabicMiddleNameTextBox.Rebind();
        this.ArabicLastNameTextBox.Rebind();
        this.ArabicFamilyNameTextBox.Rebind();
      } else if (Item.DependantType == EmployeeDependantType.Spouse) {
        if (Employee.Person.GenderTyped == Gender.Male)
          Item.Person.GenderTyped = Gender.Female;
        else
          Item.Person.GenderTyped = Gender.Male;

      }
    }

    private void GenderComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {

    }
  }

}