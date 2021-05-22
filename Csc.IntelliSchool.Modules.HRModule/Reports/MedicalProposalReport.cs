using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using Telerik.Reporting.Processing;
using Csc.Components.Common.Data;

namespace Csc.IntelliSchool.Modules.HRModule.Reports {
  public partial class MedicalProposalReport : Telerik.Reporting.Report {
    public MedicalProposalReport() {
      InitializeComponent();
    }

    private void detail_ItemDataBinding(object sender, EventArgs e) {
      var items = this.GetDataObject<IEnumerable<Employee>>(sender);
      if (items.Count() == 0)
        return;
      this.ItemsTable.SetDataSource(sender);
    }


    private void reportFooterSection1_ItemDataBinding(object sender, EventArgs e) {
      var groups = this.DataSource as IGrouping<int, Employee>[];
      var items = groups.SelectMany(s => s.ToArray()).ToArray();
      BindTotals(sender, items);
      BindFinancial(sender, items);
    }

    private void BindFinancial(object sender, Employee[] items) {
      var subItems = items.Where(s => s.MedicalCertificate != null && s.MedicalCertificate.IsActive);

      var rateTotal = subItems.Sum(s => s.MedicalInfo.ActiveRateTotal);
      var employeesMonthly = subItems.Sum(s => s.MedicalInfo.ActiveMonthlyTotal);
      var schoolMonthly = (rateTotal / 12) - employeesMonthly;

      List<KeyValue<string, int?>> values = new List<KeyValue<string, int?>>();
      values.Add(new KeyValue<string, int?>("Rate Total", rateTotal));

      foreach (var prog in subItems.Select(s => s.MedicalCertificate.Program).Distinct())
        values.Add(new KeyValue<string, int?>(prog.FullName, subItems.Where(s => s.MedicalCertificate.Program == prog).Sum(s => s.MedicalInfo.ActiveRateTotal)));
      values.Add(new KeyValue<string, int?>());

      values.Add(new KeyValue<string, int?>("Employees Monthly", employeesMonthly));
      values.Add(new KeyValue<string, int?>("School Monthly", schoolMonthly));
      values.Add(new KeyValue<string, int?>());

      // Employee Averagte Monthly
      foreach (var prog in subItems.Select(s => s.MedicalCertificate.Program).Distinct()) {
        var tmpItems = subItems.Where(s => s.MedicalCertificate.Program == prog);
        values.Add(new KeyValue<string, int?>(prog.FullName + " Emp Mthly Avg", tmpItems.Sum(s => s.MedicalInfo.ActiveMonthlyTotal) / tmpItems.Count()));
      }
        values.Add(new KeyValue<string, int?>("Total Emp Mthly Avg", employeesMonthly / subItems.Count()));
      values.Add(new KeyValue<string, int?>());

      values.Add(new KeyValue<string, int?>("Employees Yearly", employeesMonthly * 12));
      values.Add(new KeyValue<string, int?>("School Yearly", schoolMonthly * 12));

      this.FinancialTable.SetDataSource(sender, values.ToArray());
    }

    private void BindTotals(object sender, Employee[] items) {
      var subItems = items.Where(s => s.MedicalCertificate != null && s.MedicalCertificate.IsActive);

      List<KeyValue<string, int?>> values = new List<KeyValue<string, int?>>();
      values.Add(new KeyValue<string, int?>("Total", items.Count()));
      foreach (var prog in subItems.Select(s => s.MedicalCertificate.Program).Distinct())
        values.Add(new KeyValue<string, int?>(prog.FullName, subItems.Where(s => s.MedicalCertificate.Program == prog).Count()));
      values.Add(new KeyValue<string, int?>("Total Covered", subItems.Count()));
      values.Add(new KeyValue<string, int?>("Uncovered", items.Count() - subItems.Count()));
      values.Add(new KeyValue<string, int?>());


      subItems = items.Where(s => s.MedicalInfo.IsAllActive != null && s.Dependants.Count() > 0);
      var families = subItems.Count();
      var fullyCovered = subItems.Where(s => s.MedicalInfo.IsFullyCovered() == true).Count();
      values.Add(new KeyValue<string, int?>("Families", families));
      values.Add(new KeyValue<string, int?>("Covered", fullyCovered));
      values.Add(new KeyValue<string, int?>("Uncovered", families - fullyCovered));

      this.EmployeesTable.SetDataSource(sender, values.ToArray());
    }
  }
}