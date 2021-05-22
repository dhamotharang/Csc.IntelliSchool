using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Windows;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeReenrollWindow : Csc.Wpf.WindowBase {
    #region Fields
    private Employee _item;
    #endregion

    #region Properties
    public Employee Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public Employee OriginalItem { get; set; }
    #endregion

    #region Loading
    public EmployeeReenrollWindow() {
      InitializeComponent();
    }
    public EmployeeReenrollWindow(Employee item)
      : this() {
      OriginalItem = item;
      Item = item.Clone(false);
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

      this.SetBusy();
      EmployeesDataManager.ReenrollEmployee(Item, OnReenroll);
    }
    private void OnReenroll(Employee result, Exception error) {
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
