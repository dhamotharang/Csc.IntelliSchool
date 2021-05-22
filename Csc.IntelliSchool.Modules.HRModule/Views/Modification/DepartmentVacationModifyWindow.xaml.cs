using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using System.Windows;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class DepartmentVacationModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeDepartmentVacation _item;
    private DateTime? _selectedStartDate;
    private DateTime? _selectedEndDate;

    private int? _targetDepartmentID;

    // Properties
    public EmployeeDepartmentVacation Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeDepartmentVacation OriginalItem { get; set; }


    public DateTime? SelectedStartDate { get { return _selectedStartDate; } set { _selectedStartDate = value; OnPropertyChanged(() => SelectedStartDate); } }
    public DateTime? SelectedEndDate { get { return _selectedEndDate; } set { _selectedEndDate = value; OnPropertyChanged(() => SelectedEndDate); } }

    // Constructors
    public DepartmentVacationModifyWindow() {
      InitializeComponent();
    }
    public DepartmentVacationModifyWindow(int? departmentId)
      : this() {
      Item = new EmployeeDepartmentVacation();
      this.StartDatePicker.SelectedValue = null;
      this.EndDatePicker.SelectedValue = null;
      _targetDepartmentID = departmentId;
    }
    public DepartmentVacationModifyWindow(EmployeeDepartmentVacation item)
      : this() {
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone(false);
      }
    }


    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.DeleteButton.Visibility = Item.VacationID == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
      this.Header = Item.VacationID == 0 ? Csc.IntelliSchool.Assets.Resources.Text.Text_NewItem : Csc.IntelliSchool.Assets.Resources.Text.Text_UpdateItem;
      this.NameTextBox.Focus();


      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, null, OnGetDepartmentsCompletedCompleted);
    }

    private void OnGetDepartmentsCompletedCompleted(EmployeeDepartment[] result, Exception error) {
      if (result != null)
        this.DepartmentsTreeView.ItemsSource = result;
      else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void DepartmentsTreeView_ItemPrepared(object sender, Telerik.Windows.Controls.RadTreeViewItemPreparedEventArgs e) {
      var dept = (EmployeeDepartment)e.PreparedItem.Item;

      if ((null != _targetDepartmentID && _targetDepartmentID == dept.DepartmentID) || (null != OriginalItem && OriginalItem.Departments.Where(s => s.DepartmentID == dept.DepartmentID).Count() > 0))
        e.PreparedItem.CheckState = System.Windows.Automation.ToggleState.On;
    }

    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    private void SingleToggleButton_Click(object sender, RoutedEventArgs e) {
      if (this.SingleToggleButton.IsChecked == true) {
        Item.EndDate = Item.StartDate;
        this.EndDatePicker.Rebind();
        RefreshDayCount();
      }
    }
    private void StartDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (this.SingleToggleButton.IsChecked == true) {
        Item.EndDate = Item.StartDate;
        this.EndDatePicker.Rebind();
      }
      RefreshDayCount();
    }
    private void EndDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      RefreshDayCount();
    }

    private void RefreshDayCount() {
      this.DaysTextBlock.Rebind();
    }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDepartmentVacation(Item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDepartmentVacation result, Exception error) {
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
      Item.Departments.Clear();
      foreach (var itm in this.DepartmentsTreeView.CheckedItemsAs<EmployeeDepartment>())
        Item.Departments.Add(new Data.EmployeeDepartmentVacationLink() { VacationID = Item.VacationID, DepartmentID = itm.DepartmentID });

      this.SetBusy();
      if (Item.VacationID == 0)
        EmployeesDataManager.AddOrUpdateDepartmentVacations(Item, OnAdded);
      else
        EmployeesDataManager.AddOrUpdateDepartmentVacations(Item, OnUpdated);
    }

    private void OnAdded(EmployeeDepartmentVacation result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Add);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    private void OnUpdated(EmployeeDepartmentVacation result, Exception error) {
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