using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.PeopleModule.Views.Lookup {
  public partial class NationalityModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private Nationality _item;

    // Properties
    public Nationality Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public Nationality OriginalItem { get; set; }

    // Constructors
    public NationalityModifyWindow() {
      InitializeComponent();
      Item = new Nationality();
      
    }
    public NationalityModifyWindow(Nationality item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone();
        }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.NationalityID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      
      this.Header = Item.NationalityID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
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
        PeopleDataManager.DeleteNationality(Item, OnDeleted);
      });
    }

    private void OnDeleted(Nationality result, Exception error) {
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
      if (Item.NationalityID == 0)
        PeopleDataManager.AddNationality(Item, OnAdded);
      else {
        PeopleDataManager.UpdateNationality(Item, OnUpdated);
      }
    }

    private void OnAdded(Nationality result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(Nationality result, Exception error) {
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