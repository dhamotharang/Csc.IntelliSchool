using Csc.IntelliSchool.Data;
using Csc.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Modules.HRModule.Reports {
  public partial class EarningsSummaryReport : Telerik.Reporting.Report {
    public DateTime? StartMonth { get { return this.ReportParameters["StartMonth"].Value as DateTime?; } set { this.ReportParameters["StartMonth"].Value = value; } }
    public DateTime? EndMonth { get { return this.ReportParameters["EndMonth"].Value as DateTime?; } set { this.ReportParameters["EndMonth"].Value = value; } }

    public EarningsSummaryReport() {
      InitializeComponent();
    }

    private void detail_ItemDataBinding(object sender, EventArgs e) {
      var items = this.GetDataObject<IEnumerable<EmployeeEarningSummary>>(sender);
      if (items.Count() == 0)
        return;

      this.ItemsTable.SetDataSource(sender);
    }



    private void reportFooterSection1_ItemDataBinding(object sender, EventArgs e) {
      var itemGroups = (this.DataSource as IGrouping<EmployeeDepartment, EmployeeEarningSummary>[]);

      EmployeeEarningSummary[] sum = itemGroups.Select(s => EmployeeEarningSummary.Create(s.Key, s)).ToArray();

      this.SummaryTable.SetDataSource(sender, sum);
    }
  }
}