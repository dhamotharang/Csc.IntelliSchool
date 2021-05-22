using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeModifySalaryControl : Csc.Wpf.UserControlBase {
    public Employee Item { get { return DataContext as Employee; } }

    public EmployeeModifySalaryControl() {
      InitializeComponent();
    }


    private void UserControlBase_Initialized(object sender, System.EventArgs e) {

    }

    private void NumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e) {
      this.GrossTextBlock.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
    }

  }
}
