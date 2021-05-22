using Telerik.Reporting;
using Csc.Wpf;
using System.Globalization;

namespace Csc.Wpf.Views {
  public partial class ReportWindow : Csc.Wpf.WindowBase {
    private CultureInfo _currentAppCulture;

    public ReportWindow(Telerik.Reporting.Report report, object dataSource, CultureInfo culture = null) {
      InitializeComponent();
      this.CanClose = false;

      if (culture != null) {
        _currentAppCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
      }

      SetReport(report, dataSource);
    }

    private void SetReport(Report report, object dataSource) {
      InstanceReportSource src = new InstanceReportSource();
      src.ReportDocument = report;
      this.ReportViewer.ReportSource = src;

      report.DataSource = dataSource;

      this.Header = report.DocumentName;
    }

    private void ReportViewer_Error(object sender, ErrorEventArgs eventArgs) {
      Popup.AlertError(this, eventArgs.Exception);
      this.CanClose = true;
    }

    private void ReportViewer_RenderingEnd(object sender, Telerik.ReportViewer.Common.RenderingEndEventArgs args) {
      this.CanClose = true;

    }
    private void WindowBase_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (_currentAppCulture != null)
        System.Threading.Thread.CurrentThread.CurrentCulture = _currentAppCulture;
    }
  }
}
