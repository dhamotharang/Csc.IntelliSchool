
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class EmployeeMedicalCertificateGroup {
    public object Key { get; set; }
    public EmployeeMedicalCertificate[] Certificates { get; set; }
  }

}