using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;
using System.IO;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalClaimsPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<EmployeeMedicalClaim> _items;
    //private bool _needsSorting = true;
    #endregion

    #region Properties
    public override bool DataInitialized
    {
      get
      {
        return this.StatusComboBox.ItemsSource != null;
      }
    }
    public ObservableCollection<EmployeeMedicalClaim> AllItems { get; set; }
    public ObservableCollection<EmployeeMedicalClaim> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public int[] SelectedStatusIDs { get {return this.StatusComboBox.SelectedItems.Cast<EmployeeMedicalClaimStatus>().Select(s => s.StatusID).ToArray(); }}
    #endregion

    // Constructors
    public MedicalClaimsPage() {
      InitializeComponent();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      this.SetBusy();
      EmployeesDataManager.GetMedicalClaimStatuses(false, OnGetStatusListCompleted);
    }

    private void OnGetStatusListCompleted(EmployeeMedicalClaimStatus[] result, Exception error) {
      if (error == null) {
        this.StatusComboBox.ItemsSource = result;
        foreach (var itm in result.Where(s => s.IsCompletion != true))
          this.StatusComboBox.SelectedItems.Add(itm);
        OnLoadData();
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(); }
    private void LoadButton_Click(object sender, RoutedEventArgs e) { OnLoadData();      }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnLoadData();  }
    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {

    }

    private void OnLoadData() {
      if (DataInitialized == false)
        return;

      if (SelectedStatusIDs.Count() == 0) {
        OnDataLoaded(new EmployeeMedicalClaim[] { }, null);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.GetMedicalClaims(
        SelectedStatusIDs.ToArray(),
        this.FromDatePicker.SelectedDate,
        this.ToDatePicker.SelectedDate,
        OnDataLoaded);
    }
    private void OnDataLoaded(EmployeeMedicalClaim[] result, Exception error) {
      if (error == null) {
        AllItems = new ObservableCollection<EmployeeMedicalClaim>(result);
        OnFilterData();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }

    private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) { OnFilterData(); }
    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter)
        OnFilterData();
    }
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }

    private void OnFilterData() {
      if (AllItems == null)
        return;

        if (this.NameTextBox.Text.Length > 0) {
          string filterText = this.NameTextBox.Text.ToLower();

        Items = new ObservableCollection<EmployeeMedicalClaim>(AllItems.Where(s=>s.Employee.Person.FullName.ToLower().Contains(filterText) || (s.Dependant != null && s.Dependant.Person.FullName.ToLower().Contains(filterText))).ToArray());
      } else {
        Items = AllItems;
      }

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("Date", "Employee", "Dependant");
      //}
      //_needsSorting = Items.Count() == 0;
    }

    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    #region Edit
    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) { OnEditItem(); }
    private void EditButton_Click(object sender, RoutedEventArgs e) { e.SelectParentRow(); OnEditItem(); }
    private void EditMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) { OnEditItem(); }

    private void OnEditItem() {
      MedicalClaimModifyWindow wnd = new MedicalInsurance.MedicalClaimModifyWindow((EmployeeMedicalClaim)this.ItemsGridView.SelectedItem);
      this.DisplayModal<MedicalClaimModifyWindow>(wnd, OnModifyWindowClosed);
    }
    #endregion

    #region Delete
    private void DeleteButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteItem();
    }

    private void DeleteMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteItem();
    }

    private void OnDeleteItem() {
      Popup.ConfirmDelete(null, () => {
        this.SetBusy();
        var itm = (EmployeeMedicalClaim)this.ItemsGridView.SelectedItem;
        EmployeesDataManager.DeleteMedicalClaim(itm, OnDeleted);
      });
    }


    private void OnDeleted(EmployeeMedicalClaim result, Exception error) {
      if (error == null)
        Items.Remove(result);
      else
        Popup.AlertError(error);
      this.ClearBusy();
    }
    #endregion

    #region Add
    private void NewMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnNewItem();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e) {
      OnNewItem();
    }

    private void OnNewItem() {
      EmployeeSelectionWindow wnd = new EmployeeSelectionWindow(EmployeeSelectionType.Medical, null);
      this.DisplayModal<EmployeeSelectionWindow>(wnd, OnEmployeeSelected);
    }

    private void OnEmployeeSelected(EmployeeSelectionWindow obj) {
      if (obj.DialogResult != true)
        return;

      MedicalClaimModifyWindow wnd = new MedicalInsurance.MedicalClaimModifyWindow(obj.SelectedEmployee, obj.SelectedDependant);
      this.DisplayModal<MedicalClaimModifyWindow>(wnd, OnModifyWindowClosed);
    }

    private void OnModifyWindowClosed(MedicalClaimModifyWindow wnd) {
      if (wnd.DialogResult != true)
        return;

      if (wnd.Result == OperationResult.Add) {
        if (wnd.Item.StatusID == null || SelectedStatusIDs.Contains(wnd.Item.StatusID.Value)) {
          AllItems.Add(wnd.Item);
        }
      } else if (wnd.Result == OperationResult.Delete) { 
        AllItems.Remove(wnd.OriginalItem);
        Items.Remove(wnd.OriginalItem);
      } else if (wnd.Result == OperationResult.Update) {
        AllItems.Remove(wnd.OriginalItem);
        if (wnd.Item.StatusID == null || SelectedStatusIDs.Contains(wnd.Item.StatusID.Value)) {
          AllItems.Add(wnd.Item);
        }
      };

      OnFilterData();
      this.ItemsGridView.SelectItem(wnd.Item);
    }
    #endregion

    #region Context

    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("EditMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteMenuItem").IsEnabled = this.ItemsGridView.SelectedItem != null;
    }


    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }


    #endregion

  }
}