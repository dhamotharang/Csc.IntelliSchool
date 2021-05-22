using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.PeopleModule.Views.ContactInfo {
  public partial class NumberModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private ContactNumber _item;

    // Properties
    public ContactNumber Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public ContactNumber OriginalItem { get; set; }

    // Constructors
    protected NumberModifyWindow() {
      InitializeComponent();
    }
    public NumberModifyWindow(Contact contact) : this(){
      Item = new ContactNumber();
      //Item.Contact = contact;
      Item.ContactID = contact.ContactID;
    }
    public NumberModifyWindow(ContactNumber item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone();
        }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.NumberID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.NumberID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      OnLoadReferences(false);
    }

    private void OnLoadReferences(bool force) {
      this.SetBusy();
      PeopleDataManager.GetContactReferences(force, OnLoadReferencesCompleted);
    }

    private void OnLoadReferencesCompleted(string[] result, Exception error) {
      this.ClearBusy();
      if (result != null) {
        this.ReferenceComboBox.ItemsSource = result;
      } else {
        Popup.AlertError(error);
      }

    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.Close(OperationResult.Delete);
      });
    }
    #endregion

    #region Saving
    private void SaveButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      Item.TrimStrings();
      if (string.IsNullOrEmpty(Item.Reference))
        Item.Reference = PeopleDataManager.ContactDefaultReference;
      else
        Item.Reference = Item.Reference.ToTitleCase();

      if (Item.NumberID == 0) {
        this.Close(OperationResult.Add);
      } else {
        this.Close(OperationResult.Update);
      }
    }
    #endregion

    private void ReloadReferencesButton_Click(object sender, RoutedEventArgs e) {
      OnLoadReferences(true);
    }
  }

}