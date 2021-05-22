
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {



  public partial class EmployeeMedicalClaim {
    [IgnoreDataMember]
    [NotMapped]
    public decimal? ClaimedPercent {
      get {
        return ClaimedAmount != null ? (decimal)ClaimedAmount.Value / (decimal)Amount : new decimal?();
      }
    }
  }
}