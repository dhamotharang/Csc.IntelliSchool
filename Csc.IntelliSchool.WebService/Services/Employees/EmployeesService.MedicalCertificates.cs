using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Certificates
    public void DeleteEmployeeMedicalCertificate(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeMedicalCertificate>(id);
      }
    }
    
    public bool CheckEmployeeMedicalCodeUsed(int[] certificateIds, int providerId, string code) {
      if (string.IsNullOrEmpty(code)) // do not compare null codes
        return false;

      code = code.ToLower();

      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeMedicalCertificates
          .Where(s =>
            certificateIds.Contains(s.CertificateID) == false && // not the same certificate or parent
            s.Program.ProviderID == providerId &&
            s.Code != null &&
            s.Code.ToLower() == code)
          .Count() > 0;
      }
    }

    public EmployeeMedicalCertificate AddOrUpdateEmployeeMedicalCertificate(EmployeeMedicalCertificate userItem) {
      using (var ent = ServiceManager.CreateModel()) {
        //EmployeeMedicalCertificate cert = null;

        EmployeeMedicalCertificate dbItem = null;

        if (userItem.CertificateID > 0) {
          dbItem = ent.EmployeeMedicalCertificates.Find(userItem.CertificateID);
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);
        } else {
          userItem.Program = null;

          if (userItem.CertificateOwner == EmployeeMedicalCertificateOwner.Employee) {
            var emp = ent.Employees.Find(userItem.Employee.EmployeeID);
            userItem.Employees.Clear();
            emp.MedicalCertificate = userItem;
          } else if (userItem.CertificateOwner == EmployeeMedicalCertificateOwner.Dependant) {
            var dep = ent.EmployeeDependants.Find(userItem.Dependant.DependantID);
            userItem.Dependants.Clear();
            dep.MedicalCertificate = userItem;
          }
        }

        ent.SaveChanges();


        if (userItem.CertificateOwner == EmployeeMedicalCertificateOwner.Employee) {
          ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbItem != null ? SystemLogDataAction.Update : SystemLogDataAction.Insert, typeof(Employee), userItem.Employee.EmployeeID.PackArray(),
            new SystemLogDataEntry(userItem.Employee) {  });
        }
        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbItem != null ? SystemLogDataAction.Update : SystemLogDataAction.Insert, typeof(EmployeeMedicalCertificate), userItem.CertificateID.PackArray());
        ent.Logger.Flush();

        return ent.EmployeeMedicalCertificates.Include("Program.Provider").SingleOrDefault(s => s.CertificateID == userItem.CertificateID); ;
      }

    }
    #endregion


    #region Recalculation
    public void RecalculateEmployeeMedicalCertificates(int[] certificateIds, bool recalcRates, bool recalcCertificates, bool excludeCustomCertificates) {
      using (var ent = ServiceManager.CreateModel()) {
        var certs = ent.EmployeeMedicalCertificates
          .Include("Employees").Include("Dependants.Employee").Include("Program.Concessions").Include("Program.Rates")
          .Where(s => certificateIds.Contains(s.CertificateID)).ToArray();

        foreach (var cer in certs) {
          var tmpMonthly = cer.Monthly ?? 0;
          cer.Recalculate(recalcRates, recalcCertificates, excludeCustomCertificates);
        }

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Calculate, typeof(EmployeeMedicalCertificate), certs.Select(s => s.CertificateID).ToArray());

        ent.SaveChanges();
      }
    }
    #endregion


    #region Reports
    public EmployeeMedicalCertificateGroup[] GenerateEmployeeMedicalCertificates(int[] employeeIds, int[] programIds) {
      using (var ent = ServiceManager.CreateModel()) {
        var employees = ent.Employees.Include(EmployeeDataFilter.Dependants | EmployeeDataFilter.Employment).Where(s => employeeIds.Contains(s.EmployeeID)).ToArray().OrderBy(s => s.FullName).ToArray();
        var programs = ent.EmployeeMedicalPrograms.Query(EmployeeMedicalProgramDataFilter.Rates).Where(s => programIds.Contains(s.ProgramID)).ToArray();

        List<EmployeeMedicalCertificateGroup> groups = new List<EmployeeMedicalCertificateGroup>(employees.Count() + employees.SelectMany(s => s.Dependants).Count());
        foreach (var emp in employees.OrderBy(s => s.FullName)) {
          groups.Add(new EmployeeMedicalCertificateGroup() {
            Key = emp,
            Certificates = programs.Select(prog => EmployeeMedicalCertificate.Create(emp, prog)).ToArray()
          });

          foreach (var dep in emp.Dependants) {
            groups.Add(new EmployeeMedicalCertificateGroup() {
              Key = dep,
              Certificates = programs.Select(prog => EmployeeMedicalCertificate.Create(dep, prog)).ToArray()
            });
          }
        }

        return groups.ToArray();
      }
    }

    public Employee[] GenerateEmployeeMedicalProgramCertificates(int[] programIds, int[] employeeIds) {
      using (var ent = ServiceManager.CreateModel()) {
        var programs = ent.EmployeeMedicalPrograms.Query(EmployeeMedicalProgramDataFilter.Rates).Where(s => programIds.Contains(s.ProgramID)).ToArray().OrderBy(s => s.FullName).ToArray();
        var employees = ent.Employees.Include(EmployeeDataFilter.Dependants | EmployeeDataFilter.Employment)
          .Where(s => employeeIds.Contains(s.EmployeeID)).ToArray().OrderBy(s => s.FullName).ToArray();


        List<Employee> certificates = new List<Data.Employee>();
        foreach (var prog in programs) {
          foreach (var emp in employees) {
            var tmpEmp = emp.Clone(true);
            tmpEmp.MedicalCertificate = EmployeeMedicalCertificate.Create(tmpEmp, prog, DateTime.Today);
            foreach (var dep in tmpEmp.Dependants)
              dep.MedicalCertificate = EmployeeMedicalCertificate.Create(dep, prog, DateTime.Today);
            certificates.Add(tmpEmp);
          }
        }

        return certificates.ToArray();
      }
    }
    #endregion
  }


}
