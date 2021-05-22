
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeMedicalRequestDependant : IEmployeeMedicalRequestItem {
    [IgnoreDataMember]
    [NotMapped]
    public EmployeeMedicalCertificateOwner Owner {
      get { return EmployeeMedicalCertificateOwner.Dependant; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.Type {
      get { return this.Dependant.DependantType != null ? this.Dependant.DependantType.ToString() : ""; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.CertificateCode {
      get { return this.CertificateCode; }
      set { this.CertificateCode = value; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.FirstName {
      get { return this.Dependant.Person.FirstName; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.MiddleName {
      get { return this.Dependant.Person.MiddleName; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.LastName {
      get { return this.Dependant.Person.LastName; }
    }

    [IgnoreDataMember]
    [NotMapped]
    DateTime IEmployeeMedicalRequestItem.Birthdate {
      get { return this.Dependant.Person.Birthdate; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.Gender {
      get { return this.Dependant.Person.Gender; }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int OwnerID {
      get { return this.DependantID; }
    }
  }


}