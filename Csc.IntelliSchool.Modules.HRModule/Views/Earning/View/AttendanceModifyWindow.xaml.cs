using Csc.Components.Common;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using Csc.Wpf.Data;
using System;
using System.Linq;
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class AttendanceModifyWindow : Csc.Wpf.WindowBase {
    // Fields
    private EmployeeAttendance _item;
    private Employee _employee;

    // Properties
    public EmployeeAttendance Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public EmployeeAttendance OriginalItem { get; set; }
    public Employee Employee { get { return _employee; } set { _employee = value; OnPropertyChanged(() => Employee); } }


    // Constructors
    public AttendanceModifyWindow() {
      InitializeComponent();
      this.StatusComboBox.FillItems(typeof(EmployeeAttendanceStatus), false);
    }

    public AttendanceModifyWindow(Employee emp, EmployeeAttendance item)
      : this() {
      Employee = emp;
      if (item != null) {
        OriginalItem = item;
        Item = item.Clone();
      }
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.StatusComboBox.Focus();
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
      EmployeesDataManager.UpdateAttendance(Item, OnUpdated);
    }

    private void OnUpdated(EmployeeAttendance result, Exception error) {
      if (error == null) {
        Item = result;
        this.Close(OperationResult.Update);
      } else
        this.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    private void StatusComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (e.IsUser() == false)
        return;

      switch (Item.AttendanceStatus) {
        case EmployeeAttendanceStatus.Present:
          Item.InPoints = OriginalItem.InPoints;
          Item.OutPoints = OriginalItem.OutPoints;

          Item.AbsencePoints = null;
          Item.ExtraAbsencePoints = null;
          break;
        case EmployeeAttendanceStatus.Absent:
          Item.InPoints = null;
          Item.OutPoints = null;

          Item.AbsencePoints = OriginalItem.AbsencePoints;
          Item.ExtraAbsencePoints = OriginalItem.ExtraAbsencePoints;

          if (Item.AbsencePoints == null || Item.AbsencePoints == 0)
            Item.AbsencePoints = -1;
          break;
        default:
          Item.InPoints = null;
          Item.OutPoints = null;

          Item.AbsencePoints = null;
          Item.ExtraAbsencePoints = null;
          break;
      }

      RebindControls();
    }

    private void RebindControls() {
      this.InPointsNumericUpDown.Rebind();
      this.OutPointsNumericUpDown.Rebind();
      this.AbsencePointsNumericUpDown.Rebind();
      this.ExtraAbsencePointsNumericUpDown.Rebind();
    }

    private void ItemsGridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e) {
      if (e.EditAction == Telerik.Windows.Controls.GridView.GridViewEditAction.Commit) {
        var itm = ((EmployeeAttendanceTimeOff)e.EditedItem);
        if (e.EditOperationType == Telerik.Windows.Controls.GridView.GridViewEditOperationType.Insert) {
          itm.AttendanceID = Item.AttendanceID;
          itm.IsManual = true;
        }
        itm.IsEdited = true;
      }
    }

    private void ItemsGridView_Deleting(object sender, Telerik.Windows.Controls.GridViewDeletingEventArgs e) {
      var itm = ((EmployeeAttendanceTimeOff)e.Items.First());
      if (itm.IsManual == false)
        e.Cancel = true;
    }
  }

}