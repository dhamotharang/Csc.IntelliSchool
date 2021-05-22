
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {



  public partial class EmployeeSalary {

    [IgnoreDataMember]
    [NotMapped]
    public int Gross {
      get {
        return Salary +  - Medical - Social - Taxes ;
      }
    }

  }

}