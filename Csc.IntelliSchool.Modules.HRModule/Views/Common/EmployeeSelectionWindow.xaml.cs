using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeSelectionWindow : Csc.Wpf.WindowBase {
    public int? ParamListID { get { return SelectionControl.ParamListID; } set { SelectionControl.ParamListID = value; } }
    public Employee SelectedEmployee { get { return this.SelectionControl.SelectedEmployee; } }
    public EmployeeDependant SelectedDependant{ get { return this.SelectionControl.SelectedDependant; } }


    public EmployeeSelectionWindow() {
      InitializeComponent();
      SelectionControl.ItemSelected += SelectionControl_ItemSelected;
    }


    public EmployeeSelectionWindow(EmployeeSelectionType type, DateTime? month)
      : this() {
      SelectionControl.SelectionType = type;
      SelectionControl.SelectedMonth = month.ToMonth();
    }

    void SelectionControl_ItemSelected(object sender, EventArgs e) {
      OnSelected();
    }


    private void SelectButton_Click(object sender, RoutedEventArgs e) {
      OnSelected();
    }

    private void OnSelected() {
      if (SelectionControl.SelectedEmployee != null)
        this.Close(true);
      else
        this.Close(false);
    }
  }
}