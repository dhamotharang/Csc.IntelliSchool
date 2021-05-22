using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class ShiftModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeShift _item;

    // Properties
    public EmployeeShift Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeShift OriginalItem { get; set; }

    // Constructors
    public ShiftModifyWindow() {
      InitializeComponent();
      this.FromMarginTimePicker.FormatTimeSpan();
      this.ToMarginTimePicker.FormatTimeSpan();

      Item = new EmployeeShift();
    }
    public ShiftModifyWindow(EmployeeShift item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone();
        }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.ShiftID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.ShiftID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
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
        EmployeesDataManager.DeleteShift(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeShift result, Exception error) {
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

      if (Item.Validate() == false) {
        this.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Error_Validation);
        return;
      }

      Item.TrimStrings();
      Item.Name = Item.Name.ToTitleCase();

      this.SetBusy();
      if (Item.ShiftID == 0)
        EmployeesDataManager.AddOrUpdateShift(Item, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateShift(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeShift result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeShift result, Exception error) {
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