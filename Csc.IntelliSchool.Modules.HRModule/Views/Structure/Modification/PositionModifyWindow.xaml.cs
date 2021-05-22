using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class PositionModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeePosition _item;

    // Properties
    public EmployeePosition Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeePosition OriginalItem { get; set; }

    // Constructors
    public PositionModifyWindow() {
      InitializeComponent();
      Item = new EmployeePosition();
    }
    public PositionModifyWindow(EmployeePosition item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone();
        }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.PositionID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.PositionID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
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
        EmployeesDataManager.DeletePosition(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeePosition result, Exception error) {
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
      if (Item.PositionID == 0)
        EmployeesDataManager.AddPosition(Item, OnAdded);
      else {
        EmployeesDataManager.UpdatePosition(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeePosition result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeePosition result, Exception error) {
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