using Csc.Components.Common;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Lookup {
  public partial class ChargeTypeModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeChargeType _item;

    // Properties
    public EmployeeChargeType Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeChargeType OriginalItem { get; set; }

    // Constructors
    public ChargeTypeModifyWindow() {
      InitializeComponent();
      Item = new EmployeeChargeType();

    }
    public ChargeTypeModifyWindow(EmployeeChargeType item)
      : this() {
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone();
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.TypeID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

      this.Header = Item.TypeID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.NameTextBox.Focus();
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Confirm_DeleteType, () => {
        this.SetBusy();
        EmployeesDataManager.DeleteChargeType(Item, OnDeleteCompleted);
      });
    }



    private void OnDeleteCompleted(EmployeeChargeType result, Exception error) {
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
      if (Item.TypeID == 0)
        EmployeesDataManager.AddChargeType(Item, OnAdded);
      else {
        EmployeesDataManager.UpdateChargeType(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeChargeType result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeChargeType result, Exception error) {
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