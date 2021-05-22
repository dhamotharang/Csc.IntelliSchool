using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeFilterWindow : Csc.Wpf.WindowBase {
    #region Fields
    private string _title;
    private RadGridView _gridView;
    #endregion

    #region Properties
    public RadGridView GridView { get { return _gridView; } set { _gridView = value; OnPropertyChanged(() => GridView); } }
    public int? ListID { get; set; }
    public object[] Items
    {
      get
      {
        if (FilterList.IsValid == false)
          return null;

        var selectedLists = this.ListsTreeView.CheckedItemsAs<EmployeeList>().Select(s => s.ListID == 0 ? new int?() : s.ListID).ToArray();
        if (selectedLists.Count() == 0)
          return new object[] { };

        var items = this.FilterList.Items.Select(s => (object)s).AsQueryable();

        if (items.Count() > 0 && selectedLists.Count() != this.ListsTreeView.Items.Count) {
          if (items.First() is IEmployeeRelation)
            items = items.Where(s => selectedLists.Contains(((IEmployeeRelation)s).Employee.ListID));
          else if (items.First() is Employee)
            items = items.Where(s => selectedLists.Contains(((Employee)(object)s).ListID));
        }

        return items.ToArray();

      }
    }
    //public IEmployeeRelation[] Items {
    //  get {
    //    if (FilterList.IsValid == false)
    //      return null;

    //      var selectedLists = this.ListsTreeView.CheckedItemsAs<EmployeeList>().Select(s => s.ListID == 0 ? new int?() : s.ListID).ToArray();
    //      if (selectedLists.Count() == 0)
    //        return new IEmployeeRelation[] { };

    //      var items = this.FilterList.Items.Select(s => (IEmployeeRelation)s).AsQueryable();

    //      if (selectedLists.Count() != this.ListsTreeView.Items.Count) {
    //        items = items.Where(s => selectedLists.Contains(s.Employee.ListID));
    //      }

    //    return items.ToArray();
    //  }
    //}
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(() => Title); } }
    #endregion


    public EmployeeFilterWindow() {
      InitializeComponent();
    }


    public T[] ItemsAs<T>() where T: class{
      return Items.Select(s=>(T)s).ToArray();
      //if (FilterList.IsValid == false)
      //  return null;

      //var selectedLists = this.ListsTreeView.CheckedItemsAs<EmployeeList>().Select(s => s.ListID == 0 ? new int?() : s.ListID).ToArray();
      //if (selectedLists.Count() == 0)
      //  return new T[] { };

      //var items = this.FilterList.Items.Select(s => (T)s).AsQueryable();

      //if (items.Count() > 0 && selectedLists.Count() != this.ListsTreeView.Items.Count) {
      //  if (items.First() is IEmployeeRelation )
      //  items = items.Where(s => selectedLists.Contains(((IEmployeeRelation)s).Employee.ListID));
      //  else if (items.First() is Employee)
      //    items = items.Where(s => selectedLists.Contains(((Employee)(object)s).ListID));
      //}

      //return items.ToArray();
    }

    #region Loading
    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.SetBusy();
      EmployeesDataManager.GetLists(OnGetLists);
    }

    private void OnGetLists(EmployeeList[] result, Exception error) {
      if (result != null) {
        if (ListID == null)
          result = new EmployeeList[] { new EmployeeList() { Name = Csc.IntelliSchool.Assets.Resources.Text.Text_Unlisted } }.Concat(result).ToArray();
        //if (ListID > 0) {
        //  var list = result.SingleOrDefault(s => s.ListID == ListID);
        //  if (list != null)
        //    this.ListsTreeView.ItemsSource = list.PackArray();
        //} else {
        this.ListsTreeView.ItemsSource = result;
        //}
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
      if (FilterList.IsValid ==false || this.ListsTreeView.CheckedItems().Count() == 0) {
        this.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoOptionSelected);
        return;
      }

      if (Items.Count() == 0) {
        Popup.Alert(this, Csc.IntelliSchool.Assets.Resources.Text.Alert_NoSelectedItems);
        this.Close(false);
        return;
      }

      this.Close(true);
    }

    private void ListsTreeView_ItemPrepared(object sender, RadTreeViewItemPreparedEventArgs e) {
      e.PreparedItem.CheckState = System.Windows.Automation.ToggleState.On;
    }
  }
}