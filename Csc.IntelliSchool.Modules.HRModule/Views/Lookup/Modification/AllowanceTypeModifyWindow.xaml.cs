using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Lookup {
  public partial class AllowanceTypeModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeAllowanceType _item;

    // Properties
    public EmployeeAllowanceType Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeAllowanceType OriginalItem { get; set; }

    // Constructors
    public AllowanceTypeModifyWindow() {
      InitializeComponent();
      Item = new EmployeeAllowanceType();
      
    }
    public AllowanceTypeModifyWindow(EmployeeAllowanceType item)
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
      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Confirm_DeleteType,  () => {
        this.SetBusy();
        EmployeesDataManager.DeleteAllowanceType(Item, OnDeleteCompleted);
      });
    }



    private void OnDeleteCompleted(EmployeeAllowanceType result, Exception error) {
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
        EmployeesDataManager.AddAllowanceType(Item, OnAdded);
      else {
        EmployeesDataManager.UpdateAllowanceType(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeAllowanceType result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeAllowanceType result, Exception error) {
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