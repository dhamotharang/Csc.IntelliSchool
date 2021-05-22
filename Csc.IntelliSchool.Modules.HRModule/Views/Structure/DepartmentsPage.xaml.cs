using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows.Input;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class DepartmentsPage : Csc.Wpf.PageBase {
    //private RoutedUICommand _reloadCommand = new RoutedUICommand("Reload", "Reload", typeof(DepartmentsPage));
    //public RoutedUICommand ReloadCommand {
    //  get;
    //}

    //private void Reload_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
    //  e.CanExecute = true;
    //}

    //private void Reload_Executed(object sender, ExecutedRoutedEventArgs e) {
    //  OnLoadData();
    //}



    // Fields
    private ObservableCollection<EmployeeDepartment> _items;

    // Properties
    public ObservableCollection<EmployeeDepartment> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }


    // Constructors
    public DepartmentsPage() {
      InitializeComponent();
      //this.ItemsGridView.SortBy("Name");
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }

    private void OnLoadData() {
      this.SetBusy();
      EmployeesDataManager.GetDepartments(true, null, OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeDepartment[] result, Exception error) {
      if (error == null)
        Items = new ObservableCollection<EmployeeDepartment>(result);
      else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    #region Edit
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnEditItem(); }

    private void OnEditItem() {
      DepartmentModifyWindow wnd = new DepartmentModifyWindow((EmployeeDepartment)this.ItemsGridView.SelectedItem);
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }
    #endregion

    #region Delete
    private void OnDeleteItem() {
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        EmployeesDataManager.DeleteDepartment((EmployeeDepartment)this.ItemsGridView.SelectedItem, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDepartment result, Exception error) {
      if (error == null)
        Items.Remove(result);
      else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Add
    private void AddButton_Click(object sender, RoutedEventArgs e) {
      OnNewItem();
    }

    private void OnNewItem() {
      DepartmentModifyWindow wnd = new DepartmentModifyWindow();
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((DepartmentModifyWindow)sender);
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

    #region Context Menu

    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();

      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }

    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditItem();
    }

    private void NewMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnNewItem();
    }

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData();
    }

    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteItem();
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }
    #endregion

  }
}
