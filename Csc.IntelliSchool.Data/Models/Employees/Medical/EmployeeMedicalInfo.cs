
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Csc.IntelliSchool.Data {
  public class EmployeeMedicalInfo {
    public EmployeeMedicalInfo(Employee emp) {
      Employee = emp;
    }

    #region Core
    [IgnoreDataMember]
    [NotMapped]
    public Employee Employee { get; private set; }

    [IgnoreDataMember]
    [NotMapped]
    public EmployeeMedicalCertificate[] Certificates {
      get {
        List<EmployeeMedicalCertificate> list = new List<EmployeeMedicalCertificate>();
        if (Employee.MedicalCertificate != null)
          list.Add(Employee.MedicalCertificate);
        if (Employee.Dependants != null)
          list.AddRange(Employee.Dependants.Where(s => s.MedicalCertificate != null).Select(s => s.MedicalCertificate));

        return list.ToArray();
      }
    }
    #endregion

    #region Properties
    [IgnoreDataMember]
    [NotMapped]
    public bool RequiresSalaryUpdate {
      get {
        return Employee.Salary != null && Employee.Salary.Medical != (ActiveMonthlyTotal ?? 0);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? ActiveSchoolYearlyTotal {
      get {
        var certs = Certificates.Where(s => s.IsActive).ToArray();
        return certs.Sum(s => s.Yearly);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public bool? IsAllActive {
      get {
        var certs = Certificates;
        return certs.Length == 0 ? new bool?() : certs.Where(s => s.IsActive).Count() == certs.Count();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public bool? HasCustom {
      get {
        return Certificates.Length == 0 ? new bool?() : Certificates.Where(s => s.IsCustom).Count() > 0;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? ActiveRateTotal {
      get {
        return Certificates.Length == 0 ? new int?() : Certificates.Where(s => s.IsActive).Sum(s => s.Rate);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? ActiveMonthlyTotal {
      get {
        return Certificates.Length == 0 ? new int?() : Certificates.Where(s => s.IsActive).Sum(s => s.Monthly);
      }
    }
    [IgnoreDataMember]
    [NotMapped]
    public int? ActiveYearlyTotal {
      get {
        return ActiveMonthlyTotal * 12;
      }
    }
    #endregion


    #region Functions
    public bool IsFullyCovered() {
      return (Employee.Dependants.Count() + 1) == Certificates.Where(s => s.IsActive == true).Count();
    }

    public string GetSuggestedMedicalCertificateCode() {
      if (Employee.MedicalCertificate != null && string.IsNullOrEmpty(Employee.MedicalCertificate.Code) == false)
        return Employee.MedicalCertificate.Code;

      return Employee.EmployeeID.ToString();
    }
    #endregion
  }

}