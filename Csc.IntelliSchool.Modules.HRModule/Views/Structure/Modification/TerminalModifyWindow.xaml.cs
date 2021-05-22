using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class TerminalModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeTerminal _item;

    // Properties
    public EmployeeTerminal Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeTerminal OriginalItem { get; set; }

    // Constructors
    public TerminalModifyWindow() {
      InitializeComponent();
      Item = new EmployeeTerminal();
    }
    public TerminalModifyWindow(EmployeeTerminal item)
      : this() {
        if (item != null) {
          OriginalItem = item;
          Item = item.Clone() ;
        } 
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.TerminalID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.TerminalID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.NameTextBox.Focus();
      this.ModelComboBox.FillItems(typeof(EmployeeTerminalModel), true);
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteTerminal(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeTerminal result, Exception error) {
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
      if (Item.TerminalID == 0)
        EmployeesDataManager.AddOrUpdateTerminal(Item, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateTerminal(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeTerminal result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeTerminal result, Exception error) {
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