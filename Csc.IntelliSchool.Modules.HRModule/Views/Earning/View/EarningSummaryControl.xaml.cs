using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningSummaryControl : Csc.Wpf.UserControlBase, IEarningControl {
    #region  Fields
    private EmployeeEarning _item;

    private FrameworkElement[] _salaryControls;
    private FrameworkElement[] _bonusControls;
    private FrameworkElement[] _deductionControls;
    private FrameworkElement[] _absenceControls;
    private FrameworkElement[] _attendanceControls;
    private FrameworkElement[] _netControls;
    private FrameworkElement[] _allControls;
    #endregion

    #region Properties
    public Employee Employee { get { return DataContext as Employee; } }
    public EmployeeEarning Item { get { return _item; } set { _item = value; OnPropertyChanged(() => Item); } }
    public DateTime? PickedMonth { get { return this.MonthDatePicker.SelectedDate.ToMonth(); } set { this.MonthDatePicker.SelectedDate = value.ToMonth(); } }
    public DateTime? CurrentMonth { get; set; }
    public List<DateTime> UpdatedMonths { get; set; }
    public override bool HasUpdates { get { return UpdatedMonths != null && UpdatedMonths.Count > 0; } }
    public EmployeeEarningSection Section { get { return EmployeeEarningSection.Summary; } }
    #endregion


    public EarningSummaryControl() {
      InitializeComponent();
      FillControlArrays();

      UpdatedMonths = new List<DateTime>();
      this.MonthDatePicker.SetPickerTypeMonth();
      this.MonthDatePicker.SelectedDate = DateTime.Today.ToMonth();
    }

    private void FillControlArrays() {
      _salaryControls = new FrameworkElement[] {
        this.DailySalaryTextBlock,
        this.GrossTextBlock,
      };
      _bonusControls = new FrameworkElement[] {
        this.BonusPointsValueTextBlock,
        this.BonusTotalValueTextBlock,
      };
      _deductionControls = new FrameworkElement[] {
        this.DeductionPointsValueTextBlock,
        this.DeductionTotalValueTextBlock,
      };
      _absenceControls = new FrameworkElement[] {
        this.AbsenceDaysValueTextBlock,
        this.AbsenceExtraDaysValueTextBlock,
        this.UnpaidVacationsValueTextBlock,
        this.UnemploymentValueTextBlock,
        this.AbsenceTotalValueTextBlock,
      };
      _attendanceControls = new FrameworkElement[] {
        this.AttendanceValueTextBlock,
        this.TimeOffValueTextBlock,
        this.OvertimeValueTextBlock,
        this.AttendanceTotalValueTextBlock,
      };
      _netControls = new FrameworkElement[] {
        this.AdjustmentNumericUpDown,
        this.NetTextBlock
      };
      _allControls = _salaryControls
        .Concat(_bonusControls)
        .Concat(_deductionControls)
        .Concat(_absenceControls)
        .Concat(_attendanceControls)
        .Concat(_netControls).ToArray();
    }

    #region Loading
    private void UserControlBase_Initialized(object sender, System.EventArgs e) {
      OnLoadData();
    }
    private void MonthDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      OnLoadData();
    }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) {
      return;
    }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) { OnLoadData(); }
    public void OnLoadData(bool reload = false) {
      if (Employee == null || ParentTabSelected == false || (reload == false && PickedMonth == CurrentMonth))
        return;

      if (PickedMonth == null) {
        CurrentMonth = PickedMonth;
        Item = null;
        return;
      }

      this.SetBusy();
      EmployeesDataManager.GetEarnings(PickedMonth.Value, Employee.EmployeeID, OnDataLoaded);
    }

    private void RecalculateBasicButton_Click(object sender, RoutedEventArgs e) {
      if (Employee == null)
        return;

      if (CurrentMonth == null) {
        Item = null;
        return;
      }

      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Confirm_RecalculateEarning, () => {
        this.SetBusy();
        EmployeesDataManager.RecalculateEarnings(CurrentMonth.Value, Employee.EmployeeID, EmploeeEarningCalculationMode.Basic, OnDataLoaded);
      });
    }

    private void RecalculateFullMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      if (Employee == null)
        return;

      if (CurrentMonth == null) {
        Item = null;
        return;
      }

      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Confirm_RecalculateEarning, () => {
        this.SetBusy();
        EmployeesDataManager.RecalculateEarnings(CurrentMonth.Value, Employee.EmployeeID, EmploeeEarningCalculationMode.Full, OnDataLoaded);
      });
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData(true);
    }

    private void OnDataLoaded(EmployeeEarning result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        if (CurrentMonth != null && UpdatedMonths.Contains(CurrentMonth.Value) == false)
          UpdatedMonths.Add(CurrentMonth.Value);

        Item = result;
        CurrentMonth = PickedMonth;
      } else {
        Item = null;
        Popup.AlertError(error);
      }
    }
    #endregion

    #region Save
    private void UpdateButton_Click(object sender, RoutedEventArgs e) {
      if (Item == null)
        return;

      this.Confirm(Csc.IntelliSchool.Assets.Resources.HumanResources.Confirm_UpdateEarning, () => {
        this.SetBusy();
        EmployeesDataManager.UpdateEarning(Item, OnUpdated);
      });
    }

    private void OnUpdated(EmployeeEarning result, Exception error) {
      this.ClearBusy();
      if (error == null) {
        if (UpdatedMonths.Contains(CurrentMonth.Value) == false)
          UpdatedMonths.Add(CurrentMonth.Value);
        Item = result;
      } else
        Popup.AlertError(error);
    }
    #endregion

    #region UI Updates
    private void SalaryNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (_allControls != null)
        _allControls.Rebind();
    }

    private void SalaryItemNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (_salaryControls != null)
        _salaryControls.Rebind();
      RecalculateNet();
    }

    private void BonusNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (_bonusControls != null)
        _bonusControls.Rebind();
      RecalculateNet();
    }

    private void DeductionNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (_deductionControls != null)
        _deductionControls.Rebind();
      RecalculateNet();
    }

    private void LoansNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      RecalculateNet();
    }

    private void AbsenceNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (_absenceControls != null)
        _absenceControls.Rebind();
      RecalculateNet();
    }

    private void AttendanceNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      if (_attendanceControls != null)
        _attendanceControls.Rebind();
      RecalculateNet();
    }

    private void AdjustmentNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      RecalculateNet();
    }

    private void RecalculateNet() {
      Item.RecalculateAdjustment();
      if (_netControls != null)
        _netControls.Rebind();
    }
    #endregion


  }
}