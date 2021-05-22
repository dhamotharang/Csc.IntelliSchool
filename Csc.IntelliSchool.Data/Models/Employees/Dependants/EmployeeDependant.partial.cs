using Csc.Components.Common;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeDependant {
    [IgnoreDataMember]
    [NotMapped]
    public EmployeeDependantType? DependantType {
      get {
        if (this.Type == null || this.Type == string.Empty)
          return null;

        EmployeeDependantType type;
        if (Enum.TryParse<EmployeeDependantType>(Type, out type))
          return type;

        return null;
      }
      set {
        Type = value.ToString();
      }
    }

    public string GetSuggestedMedicalCertificateCode() {
      if (MedicalCertificate != null && string.IsNullOrEmpty(MedicalCertificate.Code) == false)
        return MedicalCertificate.Code;

      return EmployeeID.ToString();
    }

  }

}