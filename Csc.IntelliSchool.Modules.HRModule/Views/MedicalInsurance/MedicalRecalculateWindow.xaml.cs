using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.MedicalInsurance {
  public partial class MedicalRecalculateWindow : Csc.Wpf.WindowBase {
    private RadGridView _gridView;
    public RadGridView GridView { get { return _gridView; } set { _gridView = value; OnPropertyChanged(() => GridView); } }
    public object[] Items { get { return this.FilterList.Items != null ? this.FilterList.Items.Select(s => (Employee)s).ToArray() : null; } }
    public int[] CertificateIDs { get { return Items != null ? Items.Select(s => ((Employee)s)).SelectMany(s => s.MedicalInfo.Certificates.Select(x => x.CertificateID)).ToArray() : null; } }

    public MedicalRecalculateWindow() {
      InitializeComponent();
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {

    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }

    private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e) {
      if (CertificateIDs == null) {
        this.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoSelectedItems);
        return;
      }

      if (this.RatesCheckBox.IsChecked == false && this.ConcessionsCheckBox .IsChecked == false )
      {
        this.AlertError(Csc.IntelliSchool.Assets.Resources.Text.Alert_NoOptionSelected);
        return;
      }

      if (CertificateIDs.Count() == 0)  {
        this.Close(OperationResult.None);
        return;
      }

      this.SetBusy();
      EmployeesDataManager.RecalculateMedicalCertificates(CertificateIDs,
        this.RatesCheckBox.IsChecked == true, this.ConcessionsCheckBox.IsChecked == true, this.ExcludeCustomCheckBox.IsChecked == true,
        OnCompleted);
    }

    private void OnCompleted(Exception error) {
      if (error == null)
        this.Close(OperationResult.Update);
      else
        this.AlertError(error);
      this.ClearBusy();
    }

  }
}
