
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public partial class EmployeeMedicalRequestEmployee : IEmployeeMedicalRequestItem {

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.Type {
      get { return "Member"; }
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
      get { return this.Employee.Person.FirstName; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.MiddleName {
      get { return this.Employee.Person.MiddleName; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.LastName {
      get { return this.Employee.Person.LastName; }
    }

    [IgnoreDataMember]
    [NotMapped]
    DateTime IEmployeeMedicalRequestItem.Birthdate {
      get { return this.Employee.Person.Birthdate; }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IEmployeeMedicalRequestItem.Gender {
      get { return this.Employee.Person.Gender; }
    }

    [IgnoreDataMember]
    [NotMapped]
    EmployeeMedicalCertificateOwner IEmployeeMedicalRequestItem.Owner {
      get { return EmployeeMedicalCertificateOwner.Employee; }
    }


    [IgnoreDataMember]
    [NotMapped]
    int IEmployeeMedicalRequestItem.OwnerID {
      get { return this.EmployeeID; }
    }
  }

}