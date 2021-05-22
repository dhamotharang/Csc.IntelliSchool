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
  public partial class EmployeeModifyPersonalControl : Csc.Wpf.UserControlBase {
    public Employee Item { get { return DataContext as Employee; } }

    public EmployeeModifyPersonalControl() {
      InitializeComponent();

      this.GenderComboBox.ItemsSource = new string[] { string.Empty }.Concat(PeopleDataManager.GenderList);
      this.MaritalComboBox.ItemsSource = new string[] { string.Empty }.Concat(PeopleDataManager.MaritalStatusList);
    }


    private void UserControlBase_Initialized(object sender, System.EventArgs e) {
      OnLoadNationalities();
      OnLoadReligions();
    }

    private void BirthdateDatePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      this.BirthdateDatePicker.SetNullIfEmpty();
    }

    #region Nationality
    private void OnLoadNationalities(Nationality itemToSelect = null) {
      this.SetBusy();
      PeopleDataManager.GetNationalities(false, (res, err) => {
        this.NationalityComboBox.FillAsyncItems(res, err, a => a.Name = "", this);
        if (itemToSelect != null)
          this.NationalityComboBox.SelectedItem = itemToSelect;
      });
    }


    private void AddNationalityButton_Click(object sender, RoutedEventArgs e) {
      NationalityModifyWindow wnd = new NationalityModifyWindow();
      wnd.Closed += NationalityModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void NationalityModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadNationalities(((NationalityModifyWindow)sender).Item);
    }
    #endregion


    #region Religion
    private void OnLoadReligions(Religion itemToSelect = null) {
      this.SetBusy();
      PeopleDataManager.GetReligions(false, (res, err) => {
        this.ReligionComboBox.FillAsyncItems(res, err, a => a.Name = "", this);
        if (itemToSelect != null)
          this.ReligionComboBox.SelectedItem = itemToSelect;
      });
    }


    private void AddReligionButton_Click(object sender, RoutedEventArgs e) {
      ReligionModifyWindow wnd = new ReligionModifyWindow();
      wnd.Closed += ReligionModifyWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ReligionModifyWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;
      OnLoadReligions(((ReligionModifyWindow)sender).Item);
    }
    #endregion

  }
}
