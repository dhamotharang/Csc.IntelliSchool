using Csc.IntelliSchool.Sync.NewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Csc.IntelliSchool.Sync {
  partial class Program {
    static void Main(string[] args) {
     SyncTeachers();
    }

    private static void SyncTeachers() {
      var newEnt = new NewEntities();
      var newEmployees = newEnt.Employees.Where(s => s.HireDate <= DateTime.Today && s.TerminationDate == null && s.LocalID == null && s.ListID == null).ToArray();


      var oldEnt = new OldModel.OldEntities();

      foreach(var emp in newEmployees ){
        var oldEmp = new OldModel.employee_data();
        oldEmp.employee_name = string.Format("{0} {1} {2} {3}", emp.FirstName, emp.MiddleName, emp.LastName, emp.FamilyName).Trim().Replace("  ", " ");
       oldEmp.terminal_ip =  oldEmp.employee_nationality = oldEmp.employee_gender = oldEmp.employee_mstatus = oldEmp.employee_phone = oldEmp.employee_add = oldEmp.category = string.Empty;
        oldEmp.type_id = 38;
        oldEnt.employee_data.Add(oldEmp);
      }

      oldEnt.SaveChanges();
    }


    private static void FixData() {
      var ent = new NewEntities();

      var logs = ent.SystemLogDatas.Where(s => s.Property == "Table").Select(s => s.SystemLog).ToArray();
      foreach(var lg in logs) {
        var tableData = lg.SystemLogDatas.SingleOrDefault(s => s.Property == "Table");
        var reference = lg.SystemLogDatas.SingleOrDefault(s => s.Property == "ReferenceID");


        if (tableData != null) { 
          lg.Table = tableData.Value;
          ent.SystemLogDatas.Remove(tableData);
        }

        if (reference != null) {

          lg.References = tableData.Value;
          ent.SystemLogDatas.Remove(reference);
        }

      }

      ent.SaveChanges();
    }
    private static void SyncDocumentTypes() {
      var ent = new NewEntities();
      var docTypes = ent.EmployeeOfficialDocumentTypes.ToArray();

      foreach (var dept in ent.EmployeeDepartments.ToArray()) {
        foreach(var type in docTypes ) {
        ent.EmployeeDepartmentOfficialDocuments.Add(new EmployeeDepartmentOfficialDocument() {
          DepartmentID = dept.DepartmentID,
          TypeID = type.TypeID ,
          
        });
        }
      }

      ent.SaveChanges();
    }

    private static void SyncDocuments() {
      var newEnt = new NewEntities();
      newEnt.Configuration.AutoDetectChangesEnabled = false;
      var oldEnt = new OldModel.OldEntities();


      var papers = oldEnt.papers.ToArray();
      var newEmployees = newEnt.Employees.ToArray();

      foreach (var ppr in papers) {
        var emp = newEmployees.SingleOrDefault(s => s.LocalID == ppr.employee_id);
        if (emp == null)
          continue;

        if (ppr.graduation == true) {
          EmployeeOfficialDocument doc = new EmployeeOfficialDocument();
          doc.EmployeeID = emp.EmployeeID;
          doc.TypeID = 4;
          newEnt.EmployeeOfficialDocuments.Add(doc);
        }

        if (ppr.birth == true) {
          EmployeeOfficialDocument doc = new EmployeeOfficialDocument();
          doc.EmployeeID = emp.EmployeeID;
          doc.TypeID = 5;
          newEnt.EmployeeOfficialDocuments.Add(doc);
        }

        if (ppr.recruitment == true) {
          EmployeeOfficialDocument doc = new EmployeeOfficialDocument();
          doc.EmployeeID = emp.EmployeeID;
          doc.TypeID = 6;
          newEnt.EmployeeOfficialDocuments.Add(doc);
        }

        if (ppr.feesh == true) {
          EmployeeOfficialDocument doc = new EmployeeOfficialDocument();
          doc.EmployeeID = emp.EmployeeID;
          doc.TypeID = 3;
          newEnt.EmployeeOfficialDocuments.Add(doc);
        }

        if (ppr.id == true) {
          EmployeeOfficialDocument doc = new EmployeeOfficialDocument();
          doc.EmployeeID = emp.EmployeeID;
          doc.TypeID = 1;
          newEnt.EmployeeOfficialDocuments.Add(doc);
        }
      }

      newEnt.SaveChanges();
    }

    private static void FixFoo() {
      NewModel.NewEntities ent = new NewEntities();
      var employees = ent.Employees.Where(s => s.TerminationDate != null && s.TerminationDate.Value.Year == 2016 && s.TerminationDate.Value.Month == 12).ToArray();
      var employeeIds = employees.Select(s => s.EmployeeID).ToArray();
      var earnings = ent.EmployeeEarnings.Where(s => employeeIds.Contains(s.EmployeeID)).ToArray();

      foreach (var emp in employees) {
        var empEarnings = earnings.Where(s => s.EmployeeID == emp.EmployeeID).OrderBy(s => s.Month).LastOrDefault();

        if (empEarnings == null)
          emp.TerminationDate = new DateTime(emp.HireDate.Year, emp.HireDate.Month, 1).AddMonths(1).AddDays(-1);
        else
          emp.TerminationDate = new DateTime(empEarnings.Month.Year, empEarnings.Month.Month, 1).AddMonths(1).AddDays(-1);

      }

      ent.SaveChanges();
    }
  }
}
