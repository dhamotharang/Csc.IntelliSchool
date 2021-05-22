using Csc.IntelliSchool.Data;
using Csc.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Modules.HRModule.Reports {
  public partial class AttendanceSummaryReport : Telerik.Reporting.Report {
    public DateTime? StartMonth { get { return this.ReportParameters["StartMonth"].Value as DateTime?; } set { this.ReportParameters["StartMonth"].Value = value; } }
    public DateTime? EndMonth { get { return this.ReportParameters["EndMonth"].Value as DateTime?; } set { this.ReportParameters["EndMonth"].Value = value; } }

    public AttendanceSummaryReport() {
      InitializeComponent();
    }

    private void detail_ItemDataBinding(object sender, EventArgs e) {
      var items = this.GetDataObject<IEnumerable<EmployeeAttendanceSummary>>(sender);
      if (items.Count() == 0)
        return;

      this.ItemsTable.SetDataSource(sender);
      //this.DepartmentNameTextBox.SetText(sender, items.First().Employee.Department != null ? items.First().Employee.Department.Name : string.Empty);
    }

  }
}