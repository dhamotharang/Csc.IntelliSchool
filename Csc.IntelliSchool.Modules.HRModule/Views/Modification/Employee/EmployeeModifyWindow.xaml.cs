using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;
using Telerik.Windows.Controls;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeModifyWindow : Csc.Wpf.WindowBase {
    #region Fields
    private Employee _item;
    private int? _listID;
    #endregion

    #region Properties
    public bool? IsNewItem { get { return Item.EmployeeID == 0 ; } }
    public int? ListID { get { return _listID; } set { _listID = value; /* ModifyEmploymentControl.ListID = value; */ OnPropertyChanged(() => ListID); } }
    public Employee Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); OnPropertyChanged(() => IsNewItem); } }
    public Employee OriginalItem { get; set; }
    public EmployeeModificationSection VisibleSections { get; private set; }
    #endregion

    #region Loading
    private EmployeeModifyWindow(EmployeeModificationSection sections  = EmployeeModificationSection.Everything) {
      InitializeComponent();
    }
    public EmployeeModifyWindow(Employee item, EmployeeModificationSection sections = EmployeeModificationSection.Everything)
      : this(sections) {
      if (item != null) {
        ListID = item.ListID;
        OriginalItem = item;
        Item = item.Clone();
      }
      SetVisibleSections(sections);
    }
    public EmployeeModifyWindow(int? listID, EmployeeModificationSection sections = EmployeeModificationSection.Everything)
      : this(sections) {
      this.ListID = listID;
      Item = Employee.CreateObject();
      Item.ListID = listID ;
      SetVisibleSections(sections);
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.Header = IsNewItem == true ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
    }

    private void SetVisibleSections(EmployeeModificationSection sections ) {
      this.VisibleSections = sections;

      this.PersonalTabItem.Visibility = this.EducationTabItem.Visibility = sections.HasFlag(EmployeeModificationSection.Personal) ? Visibility.Visible : Visibility.Collapsed;
      this.ContactTabItem.Visibility = sections.HasFlag(EmployeeModificationSection.Contact) ? Visibility.Visible : Visibility.Collapsed;
      this.EmploymentTabItem.Visibility = sections.HasFlag(EmployeeModificationSection.Employment) ? Visibility.Visible : Visibility.Collapsed;
      this.OfficialDocumentsTabItem.Visibility = sections.HasFlag(EmployeeModificationSection.OfficialDocuments) ? Visibility.Visible : Visibility.Collapsed;
      this.SalaryTabItem.Visibility = sections.HasFlag(EmployeeModificationSection.Salary) ? Visibility.Visible : Visibility.Collapsed;
      this.SalaryUpdatesTabItem.Visibility = sections.HasFlag(EmployeeModificationSection.Salary) ? Visibility.Visible : Visibility.Collapsed;
      this.ExtraWindowOptionsPanel.Visibility = IsNewItem == false && sections.HasFlag(EmployeeModificationSection.Termination) ? Visibility.Visible : System.Windows.Visibility.Collapsed;
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Termination and Reeenrollment
    private void TerminateButton_Click(object sender, RoutedEventArgs e) {
      EmployeeTerminationWindow wnd = new EmployeeTerminationWindow(OriginalItem);
      wnd.Closed += EmployeeTerminationWindow_Closed;
      this.DisplayModal(wnd);
    }

    private void EmployeeTerminationWindow_Closed(object sender, WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      // DialogResult == true means successful
      Item = ((EmployeeTerminationWindow)sender).Item;
      this.Close( OperationResult.Delete);
    }

    private void ReenrollButton_Click(object sender, RoutedEventArgs e) {
      EmployeeReenrollWindow wnd = new EmployeeReenrollWindow(OriginalItem);
      wnd.Closed += EmployeeReenrollWindow_Closed;
      this.DisplayModal(wnd);
    }

    private void EmployeeReenrollWindow_Closed(object sender, WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      // DialogResult == true means successful
      Item = ((EmployeeReenrollWindow)sender).Item;
      this.Close(OperationResult.Update);
    }
    #endregion

    #region Saving
    private void SaveButton_Click(object sender, RoutedEventArgs e) {

      if (this.Validate(true) == false)
        return;

      if (Item.IsTerminalUser) {
        this.SetBusy();
        EmployeesDataManager.CheckEmployeeTerminalUsed(Item.EmployeeID, Item.TerminalID.Value, Item.TerminalUserID.Value, OnCheckTerminalUsed);
      } else {
        this.SetBusy();
        Save();
      }
    }

    private void OnCheckTerminalUsed(bool result, Exception error) {
      if (error == null) {
        if (result == false)
          Save();
        else { // true, terminal is used
          this.ClearBusy();
          Popup.AlertError(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_UserExists);
        }
      } else {
        this.ClearBusy();
        Popup.AlertError(error);
      }
    }

    private void Save() {
      // TODO: Find better way

      Item.TrimStrings();
      Item.Person.FirstName = Item.Person.FirstName.ToTitleCase();
      Item.Person.MiddleName = Item.Person.MiddleName.ToTitleCase();
      Item.Person.LastName = Item.Person.LastName.ToTitleCase();
      Item.Person.FamilyName = Item.Person.FamilyName.ToTitleCase();

      if (Item.Person.Contact != null) {
        if (Item.Person.Contact.Numbers.Count() > 0 && Item.Person.Contact.Numbers.Where(s => s.IsDefault == true).Count() == 0)
          Item.Person.Contact.Numbers.First().IsDefault = true;
        if (Item.Person.Contact.Addresses.Count() > 0 && Item.Person.Contact.Addresses.Where(s => s.IsDefault == true).Count() == 0)
          Item.Person.Contact.Addresses.First().IsDefault = true;
      } else
        Item.Person.Contact = new Contact();


      if (Item.EmployeeID == 0)
        EmployeesDataManager.AddOrUpdateEmployee(Item, OnAdded);
      else 
        EmployeesDataManager.AddOrUpdateEmployee(Item, OnUpdated);
    }

    private void OnAdded(Employee result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else {
        this.AlertError(error);
      }
      this.ClearBusy();
    }


    private void OnUpdated(Employee result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Update);
      } else {
        this.AlertError(error);
      }
      this.ClearBusy();
    }
    #endregion

    #region Photo
    private void RemovePhotoButton_Click(object sender, RoutedEventArgs e) {
      this.Confirm(Csc.IntelliSchool.Assets.Resources.Text.Photo_Confirm_Remove, (res) => {
        UpdatePhoto(null);
      });
    }

    private void UpdatePhoto(string newPhoto) {
      Item.Person.PhotoUrl = newPhoto;
      this.PhotoImage.Rebind();
    }
    private void ChangePhotoButton_Click(object sender, RoutedEventArgs e) {
      string filename = Popup.SelectFile(FileType.Images);

      if (filename == null)
        return;

      this.SetBusy();
      FileManager.UploadEmployeePhoto(filename,  OnPhotoUploaded);
    }

    private void OnPhotoUploaded(string result, Exception error) {
      if (error == null) {
        UpdatePhoto(result);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }

    private void RadTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e) {
      if (this.SalaryUpdatesTabItem.IsSelected)
        this.SalaryUpdatesControl.LoadData();
    }
    #endregion


  }

}
