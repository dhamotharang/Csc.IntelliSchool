using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class TerminalFetchWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeTerminal _item;

    // Properties
    public EmployeeTerminal Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }

    // Constructors
    public TerminalFetchWindow() {
      InitializeComponent();
    }
    public TerminalFetchWindow(EmployeeTerminal item)
      : this() {
      Item = item;
    }

     #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      
      if (Item.CanFetch == false) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_Unsupported);
        this.Close(false);
      }
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    private void FetchButton_Click(object sender, RoutedEventArgs e) {
      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_ConfirmFetch, OnFetch); 
    }

    private void OnFetch() {
      this.SetBusy();
      EmployeesDataManager.FetchTerminalTransactions(Item, this.ClearDeviceCheckBox.IsChecked == true, OnFetchCompleted);
    }

    private void OnFetchCompleted(EmployeeTerminalFetchResult result, Exception error) {
      error = error ?? result.Error;

      if (error != null) {
        this.AlertError(error ?? result.Error);
      } else {
        if (result.Success)
          Popup.Alert(string.Format(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_FetchSuccess, result.EntryCount, result.UserCount));
        else if (result.SuccessWithErrors)
          this.AlertError(string.Format(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_FetchSuccessWithError, result.EntryCount, result.UserCount), result.Error);
      }

      this.ClearBusy();

      if (error == null)
        this.Close(true);
    }
  }
}