using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class ShiftsPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeShift> _items;
    #endregion

    #region Properties
    public ObservableCollection<EmployeeShift> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    #endregion

    public ShiftsPage() {
      InitializeComponent();
      //this.ItemsGridView.SortBy("Name");
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }

    private void OnLoadData() {
      this.SetBusy();
      EmployeesDataManager.GetShifts(OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeShift[] result, Exception error) {
      if (error == null)
        Items = new ObservableCollection<EmployeeShift>(result);
      else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    #endregion

    #region Edit Shift
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnEditItem(); }

    private void OnEditItem() {
      ShiftModifyWindow wnd = new ShiftModifyWindow((EmployeeShift)this.ItemsGridView.SelectedItem);
      wnd.Closed += ShiftModifyWindow_Closed;
      this.DisplayModal(wnd);
    }
    #endregion

    #region Delete Shift
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteItem();
    }

    private void OnDeleteItem() {
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        var item = (EmployeeShift)this.ItemsGridView.SelectedItem;
        EmployeesDataManager.DeleteShift(item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeShift result, Exception error) {
      if (error == null)
        Items.Remove(result);
      else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Add Shift
    private void AddButton_Click(object sender, RoutedEventArgs e) { OnNewItem(); }


    private void OnNewItem() {
      ShiftModifyWindow wnd = new ShiftModifyWindow();
      wnd.Closed += ShiftModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ShiftModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((ShiftModifyWindow)sender);
      if (wnd.Result == OperationResult.Add) {
        Items.Add(wnd.Item);
        this.ItemsGridView.SelectItem(wnd.Item);
      } else if (wnd.Result == OperationResult.Delete)
        Items.Remove(wnd.Item);
      else if (wnd.Result == OperationResult.Update) {
        Items.Remove(wnd.OriginalItem);
        Items.Add(wnd.Item);
        this.ItemsGridView.SelectItem(wnd.Item);
      };
    }
    #endregion

    #region Shifts ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }


    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      (sender as FrameworkElement).OpenContextMenu();
    }


    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditItem();
    }

    private void NewMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnNewItem();
    }

    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteItem();
    }
    #endregion


  }
}
