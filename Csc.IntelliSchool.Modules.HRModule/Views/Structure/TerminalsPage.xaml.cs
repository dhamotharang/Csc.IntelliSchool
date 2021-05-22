using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Telerik.Windows.Controls;
using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 

namespace Csc.IntelliSchool.Modules.HRModule.Views.Structure {
  public partial class TerminalsPage : Csc.Wpf.PageBase {
    // Fields
    private ObservableCollection<EmployeeTerminal> _items;

    // Properties
    public ObservableCollection<EmployeeTerminal> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }


    // Constructors
    public TerminalsPage() {
      InitializeComponent();
      //this.ItemsGridView.SortBy("Name");
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData();
    }

    private void OnLoadData() {
      this.SetBusy();
      EmployeesDataManager.GetTerminals(OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeTerminal[] result, Exception error) {
      if (error == null)
        Items = new ObservableCollection<EmployeeTerminal>(result);
      else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    #region Edit
    private void EditButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEditItem(); }

    private void OnEditItem() {
      TerminalModifyWindow wnd = new TerminalModifyWindow((EmployeeTerminal)this.ItemsGridView.SelectedItem);
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteItem();
    }

    private void OnDeleteItem() {
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        var item = (EmployeeTerminal)this.ItemsGridView.SelectedItem;
        EmployeesDataManager.DeleteTerminal(item, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeTerminal result, Exception error) {
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
      TerminalModifyWindow wnd = new TerminalModifyWindow();
      wnd.Closed += ModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((TerminalModifyWindow)sender);
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


    #region ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();
      row?.FocusSelect();


      menu.FindMenuItem("FetchMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
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

    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnLoadData();
    }

    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteItem();
    }
    #endregion

    #region Fetch
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnFetchItem(); }
    private void FetchButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnFetchItem();
    }

    private void FetchMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnFetchItem();
    }

    private void OnFetchItem() {
      var itm = (EmployeeTerminal)this.ItemsGridView.SelectedItem;

      if (itm.CanFetch == false) {
        Popup.AlertError(Csc.IntelliSchool.Assets.Resources.HumanResources.Terminal_Unsupported);
        return;
      }

      TerminalFetchWindow wnd = new TerminalFetchWindow(itm);
      this.DisplayModal(wnd);
    }
    #endregion
  }
}
