using Csc.IntelliSchool.Sync.NewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Csc.IntelliSchool.Sync {
  partial class Program {
    private static void SyncDrivers() {
      OldModel.OldEntities oldEnt = new OldModel.OldEntities();
      NewEntities newEnt = new NewModel.NewEntities();

      oldEnt.Configuration.AutoDetectChangesEnabled = false;
      oldEnt.Configuration.ProxyCreationEnabled = false;
      newEnt.Configuration.AutoDetectChangesEnabled = false;
      newEnt.Configuration.ProxyCreationEnabled = false;


      var oldDrivers = oldEnt.bus_drivers_and_supervisors.ToArray();
      var driverPos = newEnt.EmployeePositions.Single(s => s.Name == "Driver");
      var supervisorPos = newEnt.EmployeePositions.Single(s => s.Name == "Bus Supervisor");
      var busDept = newEnt.EmployeeDepartments.Single(s => s.Name == "Buses");
      var nationality = newEnt.Nationalities.Single(s => s.Name == "Egyptian");

      var refEmployees = oldEnt.employee_data.Where(s=>s.RefID != null && s.RefType == "Driver" && s.InsuranceMonthly != null).ToArray();;
      var refIds =refEmployees.Select(s=>s.employee_id).ToArray();
      var refDependants = oldEnt.employeedependants.Where(s=>refIds.Contains(s.EmployeeID )).ToArray();

      foreach (var oldDrv in oldDrivers) {
        var emp = new Employee();
        emp.FirstName = oldDrv.FirstName;
        emp.MiddleName = oldDrv.MiddleName;
        emp.LastName = oldDrv.LastName;
        emp.FamilyName = oldDrv.FamilyName;
        emp.ArabicFirstName = string.Empty;
        emp.ArabicLastName = string.Empty;

        emp.Gender = oldDrv.user_type == 1 ? "Male" : "Female";
        emp.Birthdate = new DateTime(1900, 1, 1);
        emp.IDNumber = (oldDrv.user_social_id ?? string.Empty).Trim();
        emp.HireDate = oldDrv.employment_date != null && oldDrv.employment_date != DateTime.MinValue ? oldDrv.employment_date.Value : new DateTime(1900, 1, 1);
        emp.Department = busDept;
        emp.Position = oldDrv.user_type == 1 ? driverPos : supervisorPos;
        emp.ListID = 1;
        emp.Salary = new EmployeeSalary() {
          Salary = (int)oldDrv.salary
        };
        if (oldDrv.salary <= 2500) {
          emp.Salary.Taxes = 75;
          emp.Salary.Social = 50;
        } else if (oldDrv.salary <= 3000) {
          emp.Salary.Taxes = 100;
          emp.Salary.Social = 50;
        } else {
          emp.Salary.Taxes = 150;
          emp.Salary.Social = 50;
        }

        FillOtherData(emp, oldDrv, refEmployees, refDependants);

        newEnt.Employees.Add(emp);
      }

      newEnt.SaveChanges();
    }

    private static void FillOtherData(Employee newEmp, OldModel.bus_drivers_and_supervisors oldDrv, OldModel.employee_data[] refEmployees, OldModel.employeedependant[] refDependants) {
      var oldEmp = refEmployees.SingleOrDefault(s=>s.RefID==oldDrv.user_id);

        if (oldEmp == null)
        return;

      newEmp.Birthdate = oldEmp.employee_bd;
      newEmp.HireDate = oldEmp.employee_working_date; ;

      if (oldEmp.employee_phone != null && oldEmp.employee_phone.Trim().Length > 0) {
        newEmp.Contact = new Contact();
        newEmp.Contact.ContactNumbers.Add(new ContactNumber() {
          Reference = "Default",
          Number = oldEmp.employee_phone.Trim(),
          IsDefault = true
        });
      }

      FillMedicalInsurance(newEmp, oldEmp);
      FillDependants(newEmp, refDependants.Where(s => s.EmployeeID == oldEmp.employee_id).ToArray());
    }

  }
}
