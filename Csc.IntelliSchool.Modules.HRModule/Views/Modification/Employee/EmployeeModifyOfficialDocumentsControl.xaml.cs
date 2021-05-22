using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;
using System.Collections.ObjectModel;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeModifyOfficialDocumentsControl : Csc.Wpf.UserControlBase {
    private ObservableCollection<EmployeeOfficialDocument> _documents;

    public Employee Item { get { return DataContext as Employee; } }
    public ObservableCollection<EmployeeOfficialDocument> Documents { get { return _documents; } set { _documents = value; OnPropertyChanged(() => Documents); } }

    public EmployeeModifyOfficialDocumentsControl() {
      InitializeComponent();
    }

    private void UserControlBase_Initialized(object sender, System.EventArgs e) { OnLoadData(); }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData(true); }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { OnLoadData(); }

    public void OnLoadData(bool reload = false) {
      if (Item == null || ParentTabSelected == false || (reload == false && Documents != null && Documents.Count() > 0))
        return;

      this.SetBusy();
      EmployeesDataManager.GetOfficialDocuments(Item, OnDataLoaded);
    }

    private void OnDataLoaded(EmployeeOfficialDocument[] result, Exception error) {
      if (error == null) {
        foreach (var doc in result) {
          doc.Employee = Item;
          doc.EmployeeID = Item.EmployeeID;
        }
        Item.OfficialDocuments = result;
        Documents = new ObservableCollection<EmployeeOfficialDocument>(result);
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void ItemsGridView_BeginningEdit(object sender, Telerik.Windows.Controls.GridViewBeginningEditRoutedEventArgs e) {
      var doc = (EmployeeOfficialDocument)e.Cell.ParentRow.Item;
      doc.IsCompleted = !doc.IsCompleted;
      e.Cancel = true;
      this.ItemsGridView.Rebind();
    }

  }
}
