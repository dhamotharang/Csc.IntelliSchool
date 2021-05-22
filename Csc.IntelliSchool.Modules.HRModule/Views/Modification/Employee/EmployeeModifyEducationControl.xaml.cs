using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;
using Csc.IntelliSchool.Modules.PeopleModule.Views.Lookup;

namespace Csc.IntelliSchool.Modules.HRModule.Views {
  public partial class EmployeeModifyEducationControl : Csc.Wpf.UserControlBase {
    public Employee Item { get { return DataContext as Employee; } }

    public EmployeeModifyEducationControl() {
      InitializeComponent();
    }


    private void UserControlBase_Initialized(object sender, System.EventArgs e) {
      OnLoadDegrees();
      OnLoadFields();
    }


    #region EducationDegree
    private void OnLoadDegrees(EducationDegree itemToSelect = null) {
      this.SetBusy();
      PeopleDataManager.GetEducationDegrees(false, (res, err) => {
        this.DegreeComboBox.FillAsyncItems(res, err, a => a.Name = "", this);
        if (itemToSelect != null)
          this.DegreeComboBox.SelectedItem = itemToSelect;
      });
    }


    private void AddDegreeButton_Click(object sender, RoutedEventArgs e) {
      EducationDegreeModifyWindow wnd = new EducationDegreeModifyWindow();
      wnd.Closed += EducationDegreeModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void EducationDegreeModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadDegrees(((EducationDegreeModifyWindow)sender).Item);
    }

    #endregion


    #region EducationField
    private void OnLoadFields(EducationField itemToSelect = null) {
      this.SetBusy();
      PeopleDataManager.GetEducationFields(false, (res, err) => {
        this.FieldComboBox.FillAsyncItems(res, err, a => a.Name = "", this);
        if (itemToSelect != null)
          this.FieldComboBox.SelectedItem = itemToSelect;
      });
    }


    private void AddFieldButton_Click(object sender, RoutedEventArgs e) {
      EducationFieldModifyWindow wnd = new EducationFieldModifyWindow();
      wnd.Closed += EducationFieldModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void EducationFieldModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadFields(((EducationFieldModifyWindow)sender).Item);
    }
    #endregion


  }
}
