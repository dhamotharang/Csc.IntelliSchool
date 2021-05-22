using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using System.Windows;
using System.Collections.ObjectModel;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Documents {
  public partial class DocumentsWindow : Csc.Wpf.WindowBase {
    #region Fields
    private Employee _employee;
    private ObservableCollection<EmployeeDocument> _documents;
    #endregion

    #region Properties
    public Employee Employee { get { return _employee; } protected set { _employee = value; OnPropertyChanged(() => Employee); } }
    public ObservableCollection<EmployeeDocument> Items { get { return _documents; } set { _documents = value; OnPropertyChanged(() => Items); } }
    #endregion

    #region Loading
    public DocumentsWindow() {
      InitializeComponent();
      //this.ItemsGridView.SortBy("Name");

    }
    public DocumentsWindow(Employee itm)
      : this() {
      Employee = itm;

    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      OnLoadData();
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) {
      OnLoadData();
    }
    private void OnLoadData() {
      this.SetBusy();
      EmployeesDataManager.GetDocuments(Employee.EmployeeID, OnLoaded);
    }

    private void OnLoaded(EmployeeDocument[] result, Exception error) {
      if (error == null)
        Items = new ObservableCollection<EmployeeDocument>(result.OrderBy(s => s.Name).ToArray());
      else
        this.AlertError(error);
      this.ClearBusy();
    }

    #endregion

    #region Basic
    private void OKButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }
    #endregion

    #region View or Open
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {OnOpenItem(); }

    private void OpenButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnOpenItem(); }
    private void OnOpenItem() {
      var itm = this.ItemsGridView.SelectedItemAs<EmployeeDocument>();

      if (itm.GetFullUrl() == null)
        return;

        ProcessExtensions.Start(itm.GetFullUrl());

    }

    private void SaveButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnSaveItem(); }
    private void OnSaveItem() {
      var itm = this.ItemsGridView.SelectedItemAs<EmployeeDocument>();

      if (itm.GetFullUrl() == null)
        return;

      try {
        var file = Popup.SaveFile(itm.GetFullUrl());
        if (file != null)
        Popup.Alert(string.Format(Csc.IntelliSchool.Assets.Resources.Text.File_Saved, file));
      } catch (Exception ex) {
        this.AlertError(ex);
      }
    }
    #endregion

    #region Modify
    private void AddButton_Click(object sender, RoutedEventArgs e) {
      OnNewItem();
    }

    private void OnNewItem() {
      DocumentModifyWindow wnd = new DocumentModifyWindow(Employee);
      this.DisplayModal<DocumentModifyWindow>(wnd, OnDocumentWindowClosed);
    }

    private void EditButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow() ;

      OnEditItem();
    }

    private void OnEditItem() {
      DocumentModifyWindow wnd = new DocumentModifyWindow(this.ItemsGridView.SelectedItem as EmployeeDocument);
      this.DisplayModal<DocumentModifyWindow>(wnd, OnDocumentWindowClosed);
    }


    private void OnDocumentWindowClosed(DocumentModifyWindow obj) {
      if (obj.Result == OperationResult.None)
        return;

      if (obj.Result == OperationResult.Add)
        Items.Add(obj.Item);
      else if (obj.Result == OperationResult.Update) {
        Items.Remove(obj.OriginalItem);
        Items.Add(obj.Item);
      } else if (obj.Result == OperationResult.Delete)
        Items.Remove(obj.OriginalItem);

      this.ItemsGridView.Rebind();
    }


    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteItem();
    }

    private void OnDeleteItem() {
      var itm = this.ItemsGridView.SelectedItemAs<EmployeeDocument>();
      this.ConfirmDelete(() => {
        this.SetBusy();
        EmployeesDataManager.DeleteDocument(itm, OnDeleted);
      });
    }

    private void OnDeleted(EmployeeDocument result, Exception error) {
      if (error == null) {
        Items.Remove(result);
        this.ItemsGridView.Rebind();
      } else
        this.AlertError(error);

      this.ClearBusy();
    }
    #endregion

    #region ContextMenu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();



      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("OpenMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("SaveMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
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

    private void OpenMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnOpenItem();
    }

    private void SaveMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnSaveItem();
    }
    #endregion

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }
  }
}
