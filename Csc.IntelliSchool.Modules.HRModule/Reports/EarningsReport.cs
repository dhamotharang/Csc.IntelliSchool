using Csc.IntelliSchool.Data;
using Csc.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Modules.HRModule.Reports {
  public partial class EarningsReport : Telerik.Reporting.Report {
    public DateTime? Month { get { return this.ReportParameters["Month"].Value as DateTime?; } set { this.ReportParameters["Month"].Value = value; } }

    public EarningsReport() {
      InitializeComponent();
    }

    private void detail_ItemDataBinding(object sender, EventArgs e) {
      var items = this.GetDataObject<IEnumerable<EmployeeEarning>>(sender);
      if (items.Count() == 0)
        return;

      this.ItemsTable.SetDataSource(sender);
    }


    private void reportFooterSection1_ItemDataBinding(object sender, EventArgs e) {
      var itemGroups = (this.DataSource as IGrouping<EmployeeDepartment, EmployeeEarning>[]);

      EmployeeEarningSummary[] sum = itemGroups.Select(s => EmployeeEarningSummary.Create(s.Key, s)).OrderBy(s => s.Key).ToArray();
      
      this.SummaryTable.SetDataSource(sender, sum);
    }
  }
}

