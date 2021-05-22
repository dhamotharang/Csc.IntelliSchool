using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalProposalListWindow : Csc.Wpf.WindowBase {
    private Employee[] _allItems;
    private Employee[] _items;
    //private bool _needsSorting = true;

    public Employee[] AllItems { get { return _allItems; } set { if (_allItems != value) { _allItems = value; OnPropertyChanged(() => AllItems); OnFilterData(); } } }
    public Employee[] Items { get { return _items; } private set { if (_items != value) { _items = value; OnPropertyChanged(() => Items); } } }

    public MedicalProposalListWindow() {
      InitializeComponent();
      //this.ItemsGridView.GroupBy("Program");
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {

    }


    #region Filteration
    private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter) {
        OnFilterData();
      }
    }
    private void FilterButton_Click(object sender, RoutedEventArgs e) { OnFilterData(); }
    private void OnFilterData() {
      if (AllItems == null) {

        Items = null;
        return;
      }

      if (this.NameTextBox.Text.Length > 0) {
        string filterText = this.NameTextBox.Text.ToLower();

        Items = AllItems.Where(s => s.Person.FullName.ToLower().Contains(filterText) || s.Dependants.Select(x => x.Person.FullName.ToLower()).Any(y => y.Contains(filterText))).ToArray();
      } else
        Items = AllItems;

      //if (_needsSorting && this.ItemsGridView.SortDescriptors.Count() == 0) {
      //  this.ItemsGridView.SortBy("Name");
      //}
      //_needsSorting = Items.Count() == 0;
    }
    #endregion

    #region Other
    private void ItemsGridView_DataLoaded(object sender, EventArgs e) {
      this.ItemsGridView.ExpandAllGroups();
    }
    #endregion

    private void ItemsGridView_Loaded(object sender, RoutedEventArgs e) {
      //this.GridColumnFilterPanel.FilterColumnGroups(this.ItemsGridView, "Name", "Employment", "Position");

    }

    private void PrintListMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      PrintButton.IsOpen = false;



      EmployeeFilterWindow wnd = new EmployeeFilterWindow();
      wnd.GridView = this.ItemsGridView;
      this.DisplayModal<EmployeeFilterWindow>(wnd, OnPrintList);
    }

    private void OnPrintList(EmployeeFilterWindow obj) {
      if (obj.DialogResult != true)
        return;

      var items = obj.ItemsAs<Employee>().ToArray();
      items = items.Where(s => s.MedicalCertificate != null).OrderBy(s => s.MedicalCertificate.Program.FullName).Concat(items.Where(s => s.MedicalCertificate == null)).ToArray();

      this.DisplayReport(new Reports.MedicalProposalReport(), items.GroupBy(s => s.MedicalCertificate != null ? s.MedicalCertificate.ProgramID : 0).ToArray(), null);
    }
  }
}
