
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {



  public partial class EmployeeMedicalProgram {

    [IgnoreDataMember]
    [NotMapped]
    public string FullName {
      get {
        return string.Format("{0} {1}", Provider != null ? Provider.Name : string.Empty, Name).Trim();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string LongName {
      get {
        return string.IsNullOrEmpty(PolicyCode) ? FullName : string.Format("{0} ({1})", FullName, PolicyCode);
      }
    }

    public int? GetRate(int age, EmployeeMedicalCertificateOwner owner) {
      var rate = this.Rates.OrderBy(s => s.MinAge).ThenBy(s => s.RateID).Where(s => s.MinAge <= age && s.MaxAge >= age).LastOrDefault();
      if (rate == null)
        return null;
      if (owner == EmployeeMedicalCertificateOwner.Employee)
        return rate.EmployeeValue;
      else if (owner == EmployeeMedicalCertificateOwner.Dependant)
        return rate.DependantValue;
      return null;
    }

    public decimal? GetConcession(int? employmentYears, EmployeeMedicalCertificateOwner rateType) {
      if (employmentYears == null)
        return null;

      var conc = this.Concessions.OrderBy(s => s.MinYears).ThenBy(s => s.ConcessionID).Where(s => s.MinYears <= employmentYears.Value).LastOrDefault();
      if (conc == null)
        return null;

      if (rateType == EmployeeMedicalCertificateOwner.Employee)
        return conc.EmployeePercent;
      else if (rateType == EmployeeMedicalCertificateOwner.Dependant)
        return conc.DependantPercent;
      return null;
    }

    public int? GetMaximumConcession(int? employmentYears, EmployeeMedicalCertificateOwner rateType) {
      if (employmentYears == null)
        return null;

      var conc = this.Concessions.OrderBy(s => s.MinYears).ThenBy(s => s.ConcessionID).Where(s => s.MinYears <= employmentYears.Value).LastOrDefault();
      if (conc == null)
        return null;

      if (rateType == EmployeeMedicalCertificateOwner.Employee)
        return conc.EmployeeMaximumAmount;
      else if (rateType == EmployeeMedicalCertificateOwner.Dependant)
        return conc.DependantMaximumAmount;
      return null;
    }
  }



}