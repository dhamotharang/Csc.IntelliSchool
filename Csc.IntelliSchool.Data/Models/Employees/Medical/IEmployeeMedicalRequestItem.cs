using System;
namespace Csc.IntelliSchool.Data {
  public interface IEmployeeMedicalRequestItem {
    EmployeeMedicalCertificateOwner Owner { get; }
    int OwnerID { get; }
    string Type { get; }
    string CertificateCode { get; set; }
    string FirstName { get; }
    string MiddleName { get; }
    string LastName { get; }
    DateTime Birthdate { get; }
    string Gender { get; }
  }


}