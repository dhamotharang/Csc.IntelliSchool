using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeModifyEmploymentControl : Csc.Wpf.UserControlBase {

    public Employee Item { get { return DataContext as Employee; } }


    public int? ListID {
      get { return (int?)GetValue(ListIDProperty); }
      set { SetValue(ListIDProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ListID.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ListIDProperty =
        DependencyProperty.Register("ListID", typeof(int?), typeof(EmployeeModifyEmploymentControl), new PropertyMetadata(null));



    public EmployeeModifyEmploymentControl() {
      InitializeComponent();
    }

    private void UserControlBase_Initialized(object sender, System.EventArgs e) {
 }

    private void HireStartDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      this.HireStartDatePicker.SetNullIfEmpty();
    }

    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) {
      if (DataLoaded )
        return;

      this.SetBusy();
      EmployeesDataManager.GetBranches(false, (res, err) => this.BranchComboBox.FillAsyncItems(res, err, a => a.Name = "", this));
      this.SetBusy();
      EmployeesDataManager.GetDepartments(false, ListID, (res, err) => this.DepartmentComboBox.FillAsyncItems(res, err, a => a.Name = "", this));
      this.SetBusy();
      EmployeesDataManager.GetPositions(false, ListID, (res, err) => this.PositionComboBox.FillAsyncItems(res, err, a => a.Name = "", this));
      this.SetBusy();
      EmployeesDataManager.GetShifts ((res, err) => this.ShiftComboBox.FillAsyncItems(res != null ? res.OrderBy(s => s.Name).ToArray() : null, err, a => a.Name = "", this));
      this.SetBusy();
      EmployeesDataManager.GetTerminals((res, err) => this.TerminalComboBox.FillAsyncItems(res != null ? res.OrderBy(s => s.Name).ToArray() : null, err, a => a.Name = "", this));
      this.SetBusy();
      EmployeesDataManager.GetLists((res, err) => this.ListComboBox.FillAsyncItems(res != null ? res.OrderBy(s => s.Name).ToArray() : null, err, a => a.Name = "", this));

      DataLoaded = true;
    }

  }
}