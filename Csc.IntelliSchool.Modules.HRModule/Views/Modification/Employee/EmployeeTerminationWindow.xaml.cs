using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;
using System;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeTerminationWindow : Csc.Wpf.WindowBase {
    #region Fields
    private Employee _item;
    #endregion

    #region Properties
    public Employee Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public Employee OriginalItem { get; set; }
    #endregion


    #region Loading
    public EmployeeTerminationWindow() {
      InitializeComponent();
    }
    public EmployeeTerminationWindow(Employee item)
      : this() {
      OriginalItem = item;
      Item = item.Clone(false);
      Item.TerminationHide = false;
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Saving
    private void SaveButton_Click(object sender, RoutedEventArgs e) {
      if (this.Validate(true) == false)
        return;

      if (Item.TerminationReason != null)
        Item.TerminationReason = Item.TerminationReason.Trim();

      this.SetBusy();
      EmployeesDataManager.TerminateEmployee(Item, OnTerminated);
    }
    private void OnTerminated(Employee result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Delete);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

  }

}
