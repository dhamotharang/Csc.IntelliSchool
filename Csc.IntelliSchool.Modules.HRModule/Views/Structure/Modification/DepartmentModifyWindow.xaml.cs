using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class DepartmentModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeDepartment _item;

    // Properties
    public EmployeeDepartment Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeDepartment OriginalItem { get; set; }

    // Constructors
    public DepartmentModifyWindow() {
      InitializeComponent();
      Item = new EmployeeDepartment();
    }
    public DepartmentModifyWindow(EmployeeDepartment item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone();
        }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.DepartmentID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.DepartmentID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
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
        EmployeesDataManager.DeleteDepartment(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDepartment result, Exception error) {
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
      if (Item.DepartmentID == 0)
        EmployeesDataManager.AddDepartment(Item, OnAdded);
      else {
        EmployeesDataManager.UpdateDepartment(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeDepartment result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeDepartment result, Exception error) {
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