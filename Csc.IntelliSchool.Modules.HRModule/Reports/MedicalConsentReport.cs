using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Drawing;
using Csc.Wpf;
using Csc.Components.Common;

namespace Csc.IntelliSchool.Modules.HRModule.Reports {
  public partial class MedicalConsentReport : Telerik.Reporting.Report {
    private Image ConsentImage { get; set; }
    public MedicalConsentReport() {
      InitializeComponent();
    }

    private void detail_ItemDataBinding(object sender, EventArgs e) {
      var items = this.GetDataObject<EmployeeMedicalCertificateGroup>(sender);
      this.ItemsTable.SetDataSource(sender, items.Certificates);

      Employee emp = items.Key as Employee;
      EmployeeDependant dep = items.Key as EmployeeDependant;

      DateTime birthdate = DateTime.MinValue, hireDate = DateTime.MinValue;
      int age = 0, hireYears = 0;


      if (emp != null) {
        birthdate = emp.Person.Birthdate;
        age = emp.Person.Age;
        hireDate = emp.HireDate;
        hireYears = emp.HireYears;
      } else {
        birthdate = dep.Person.Birthdate;
        age = dep.Person.Age;
      }

      this.BirthdateTextBox.SetValue(sender, string.Format("{0:d} - {1} Year(s)", birthdate, age));
      if (emp != null) {
        this.PositionTextBox.SetValue(sender, emp.FullPositionString);
        this.HireTextBox.SetValue(sender, string.Format("{0:d} - {1} Year(s)", hireDate, hireYears));
      } else {
        this.PositionTextBox.Visible = false;
        this.HireTextBox.SetValue(sender, "");
        this.HireTitleTextBox.SetValue(sender, "");
      }
    }

  }
}