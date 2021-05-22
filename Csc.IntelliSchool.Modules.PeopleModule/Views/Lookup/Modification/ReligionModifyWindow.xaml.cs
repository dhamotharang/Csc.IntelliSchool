using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.PeopleModule.Views.Lookup {
  public partial class ReligionModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private Religion _item;

    // Properties
    public Religion Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public Religion OriginalItem { get; set; }

    // Constructors
    public ReligionModifyWindow() {
      InitializeComponent();
      Item = new Religion();
      
    }
    public ReligionModifyWindow(Religion item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone();
        }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.ReligionID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      
      this.Header = Item.ReligionID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.NameTextBox.Focus();
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        PeopleDataManager.DeleteReligion(Item, OnDeleted);
      });
    }

    private void OnDeleted(Religion result, Exception error) {
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
      Item.Name = Item.Name.ToTitleCase();

      this.SetBusy();
      if (Item.ReligionID == 0)
        PeopleDataManager.AddReligion(Item, OnAdded);
      else {
        PeopleDataManager.UpdateReligion(Item, OnUpdated);
      }
    }

    private void OnAdded(Religion result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(Religion result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion
  }

}