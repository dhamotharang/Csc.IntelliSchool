using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using Csc.Wpf;
using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {

  public partial class EarningWindow : Csc.Wpf.WindowBase {
    #region Fields
    private Employee _item;
    #endregion

    #region Properties
    public Employee Item { get { return _item; } protected set { _item = value; OnPropertyChanged(() => Item); } }
    public IEarningControl[] Components { get; private set; }
    public override bool HasUpdates { get { return Components.Any(s => s.HasUpdates); } }
    #endregion
    
    #region Loading
    public EarningWindow(DateTime? month = null, EmployeeEarningSection sections = EmployeeEarningSection.All) {
      InitializeComponent();

      Components = new IEarningControl[] {
        this.AttendanceControl,
        this.AllowancesControl ,
        this.ChargesControl ,
        this.BonusesControl,
        this.DeductionsControl,
        this.VacationsControl,
        this.DepartmentVacationsControl,
        this.LoansControl,
        this.SummaryControl,
        this.HistoryControl,
      };

      foreach (var ctl in Components) {
        ctl.PickedMonth = month ?? DateTime.Today.ToMonth();
      }
    }
    public EarningWindow(Employee item, DateTime? month = null, EmployeeEarningSection sections = EmployeeEarningSection.All)
      : this(month, sections) {

      Item = item;
      SetVisibleFields(sections);
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
    }

    private void SetVisibleFields(EmployeeEarningSection sections) {
      foreach (var ctl in Components) {
        (ctl as UserControlBase).Visibility = sections.HasFlag(ctl.Section) ? Visibility.Visible : Visibility.Collapsed;
      }
    }
    #endregion

    #region Basic
    void ModifyControl_BusyChanged(object sender, EventArgs e) { this.SetBusy(((IBusy)sender).IsBusy); }
    private void OKButton_Click(object sender, RoutedEventArgs e) { this.Close(true, OperationResult.None); }
    #endregion

    private void RadTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e) {
      if (e.AddedItems.Count == 0)
        return;

      var tab = e.AddedItems[0] as RadTabItem;

      var earningCtl =  (tab.FindLogicalChild <UserControlBase>() as IEarningControl);
      if (earningCtl != null)
        earningCtl.OnLoadData();
    }
  }

}
