using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {


  public partial class EmployeeMedicalCertificate {

    public static EmployeeMedicalCertificate Create(object owner, EmployeeMedicalProgram prog, DateTime? enrollment = null) {
      EmployeeMedicalCertificate cert = new Data.EmployeeMedicalCertificate();
      cert.SetOwner(owner);
      cert.EnrollmentDate = enrollment;
      cert.Recalculate(prog);
      return cert;
    }

    #region Owner
    [IgnoreDataMember]
    [NotMapped]
    public Employee Employee { get { return Employees.FirstOrDefault(); } }
    [IgnoreDataMember]
    [NotMapped]
    public EmployeeDependant Dependant { get { return Dependants.FirstOrDefault(); } }
    [IgnoreDataMember]
    [NotMapped]
    public object OwnerObject { get { return Employee != null ? (object)Employee : (object)Dependant; } }

    [IgnoreDataMember]
    [NotMapped]
    public EmployeeMedicalCertificateOwner? CertificateOwner {
      get {
        EmployeeMedicalCertificateOwner owner;
        if (Enum.TryParse<EmployeeMedicalCertificateOwner>(Owner, out owner))
          return owner;
        return null;
      }
      set { Owner = value != null ? value.ToString() : null; }
    }

    [IgnoreDataMember]
    [NotMapped]
    public EmployeeMedicalCertificateOwner? CertificateRateType {
      get {
        EmployeeMedicalCertificateOwner owner;
        if (Enum.TryParse<EmployeeMedicalCertificateOwner>(RateType, out owner))
          return owner;
        return null;
      }
      set { RateType = value != null ? value.ToString() : null; }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? OwnerHireYears {
      get {
        if (CertificateOwner == EmployeeMedicalCertificateOwner.Dependant && Dependant != null)
          return Dependant.Employee.HireYears;
        else if (CertificateOwner == EmployeeMedicalCertificateOwner.Employee && Employee != null)
          return Employee.HireYears;
        else
          return null;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? OwnerAge {
      get {
        if (CertificateOwner == EmployeeMedicalCertificateOwner.Dependant && Dependant != null)
          return Dependant.Person.Age;
        else if (CertificateOwner == EmployeeMedicalCertificateOwner.Employee && Employee != null)
          return Employee.Person.Age;
        else
          return null;
      }
    }

    #endregion Owner

    [IgnoreDataMember]
    [NotMapped]
    public bool IsActive { get { return EnrollmentDate != null && CancellationDate == null && Rate != null; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool IsCustom { get { return Concession == null || CertificateRateType != CertificateOwner; } }
    [IgnoreDataMember]
    [NotMapped]
    public int? Yearly { get { return Monthly != null ? Monthly.Value * 12 : new int?(); } }

    [IgnoreDataMember]
    [NotMapped]
    public int? SchoolYearly { get { return Monthly != null ? Rate - Yearly : new int?(); } }

    public void SetOwner(object owner) {
      if (owner is Employee) {
        Employees.Add((Employee)owner);
        CertificateOwner = EmployeeMedicalCertificateOwner.Employee;
        CertificateRateType = EmployeeMedicalCertificateOwner.Employee;
      } else if (owner is EmployeeDependant) {
        Dependants.Add((EmployeeDependant)owner);
        CertificateOwner = EmployeeMedicalCertificateOwner.Dependant;
        CertificateRateType = EmployeeMedicalCertificateOwner.Dependant;
      }
    }

    public void Recalculate(bool recalcRate, bool recalcConcession, bool excludeCustomConcessions) {
      Recalculate(Program, recalcRate, recalcConcession, excludeCustomConcessions);
    }

    public void Recalculate(EmployeeMedicalProgram prog) {
      Recalculate(prog, true, true, false);
    }

    public void Recalculate(EmployeeMedicalProgram prog, bool recalcRate, bool recalcConcession, bool excludeCustomConcessions) {
      Program = prog;
      ProgramID = prog != null ? prog.ProgramID : 0;

      if (prog == null || CertificateRateType == null) {
        Rate = null;
        Monthly = null;
        return;
      }

      if (recalcRate) { // set rate based on Owner Type, value cannot be edited by user
        this.Rate = prog.GetRate(OwnerAge.Value, CertificateRateType.Value);
      }

      if (recalcConcession && (Concession != null || excludeCustomConcessions == false)) {
        this.Concession = prog.GetConcession(OwnerHireYears, CertificateOwner.Value);
        this.MaximumConcession = prog.GetMaximumConcession(OwnerHireYears, CertificateOwner.Value);
      }

      CalculateMonthly();
    }

    public void CalculateMonthly() {
      if (Rate == null) {
        Monthly = null;
        return;
      }

      if (Concession == null)
        return;

      if (Rate == 0) {
        Monthly = 0;
        return;
      }

      var conc = Math.Round((decimal)Rate * Concession.Value);
      if (this.MaximumConcession != null && conc > this.MaximumConcession)
        conc = this.MaximumConcession.Value;

      var yearly = Rate.Value - conc;

      Monthly = (int)Math.Round((double)yearly / 12.0);
    }
  }

}