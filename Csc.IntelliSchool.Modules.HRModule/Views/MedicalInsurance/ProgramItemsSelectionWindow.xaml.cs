using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class ProgramItemsSelectionWindow : Csc.Wpf.WindowBase {
    #region Fields
    private RadGridView _gridView;
    #endregion

    #region Properties
    private bool ShowSystemPrograms { get; set; }
    public RadGridView GridView { get { return _gridView; } set { _gridView = value; OnPropertyChanged(() => GridView); } }
    public int? ListID { get; set; }
    public Employee[] Items
    {
      get
      {
        if (FilterList.IsValid == false)
          return null;

        return this.FilterList.Items.Select(s => (Employee)s).ToArray();
      }
    }
    public EmployeeMedicalProgram[] Programs
    {
      get
      {
        return this.ProgramsTreeView.CheckedItemsAs<EmployeeMedicalProgram>();
      }
    }
    #endregion


    public ProgramItemsSelectionWindow(bool showSystem = true) {
      InitializeComponent();
      ShowSystemPrograms = showSystem;
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.SetBusy();
      EmployeesDataManager.GetMedicalPrograms(false, OnGetPrograms);
    }

    private void OnGetPrograms(EmployeeMedicalProgram[] result, Exception error) {
      if (result != null) {
        this.ProgramsTreeView.ItemsSource = result.Where(s => ShowSystemPrograms || s.IsSystem == false).ToArray() ;
      } else {
        Popup.AlertError(error);
      }
      this.ClearBusy();
    }
    #endregion

    #region Basic
    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion


    private void SelectButton_Click(object sender, RoutedEventArgs e) {
      if (FilterList.IsValid == false || this.ProgramsTreeView.CheckedItems().Count() == 0 || Items.Count() == 0) {
        this.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoSelectedItems);
        return;
      }

      this.Close(true);
    }

    private void ProgramsTreeView_ItemPrepared(object sender, RadTreeViewItemPreparedEventArgs e) {
      e.PreparedItem.CheckState = System.Windows.Automation.ToggleState.On;
    }
  }
}