using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class TransactionRuleModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeTransactionRule _item;

    // Properties
    public EmployeeTransactionRule Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeTransactionRule OriginalItem { get; set; }

    // Constructors
    public TransactionRuleModifyWindow() {
      InitializeComponent();
      this.TimePicker.FormatTimeSpan();

      Item = new EmployeeTransactionRule();
      this.TypeInRadioButton.IsChecked = true;
    }
    public TransactionRuleModifyWindow(EmployeeTransactionRule item)
      : this() {
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone();
        switch (item.RuleType) {
          case EmployeeTransactionRuleType.In:
            this.TypeInRadioButton.IsChecked = true;
            break;
          case EmployeeTransactionRuleType.Out:
            this.TypeOutRadioButton.IsChecked = true;
            break;
          case EmployeeTransactionRuleType.TimeOff:
            this.TypeDuringRadioButton.IsChecked = true;
            break;
          case EmployeeTransactionRuleType.Overtime:
            this.TypeOvertimeRadioButton.IsChecked = true;
            break;
        }
      }
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.RuleID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.RuleID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
    }
    #endregion

    private void TypeRadioButton_Click(object sender, RoutedEventArgs e) {

    }

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteTransactionRule(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeTransactionRule result, Exception error) {
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

      if (this.TypeInRadioButton.IsChecked == true)
        Item.RuleType = EmployeeTransactionRuleType.In;
      else if (this.TypeOutRadioButton.IsChecked == true)
        Item.RuleType = EmployeeTransactionRuleType.Out;
      else if (this.TypeDuringRadioButton.IsChecked == true)
        Item.RuleType = EmployeeTransactionRuleType.TimeOff;
      else
        Item.RuleType = EmployeeTransactionRuleType.Overtime;

      Item.Time = new TimeSpan(Item.Time.Hours, Item.Time.Minutes, 0);

      this.SetBusy();
      if (Item.RuleID == 0)
        EmployeesDataManager.AddOrUpdateTransactionRule(Item, OnAdded);
      else {
        EmployeesDataManager.AddOrUpdateTransactionRule(Item, OnUpdated);
      }
    }

    private void OnAdded(EmployeeTransactionRule result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeTransactionRule result, Exception error) {
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