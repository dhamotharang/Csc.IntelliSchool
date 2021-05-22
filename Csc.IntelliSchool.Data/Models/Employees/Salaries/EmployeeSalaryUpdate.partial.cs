
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeSalaryUpdate {

    [IgnoreDataMember]
    [NotMapped]
    public int Gross {
      get {
        return Salary + - Medical - Social - Taxes ;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? PreviousSalary { get; set; }

    [IgnoreDataMember]
    [NotMapped]
    public int? SalaryIncrease {
      get {
        if (PreviousSalary == null)
          return null;

        return (Salary - PreviousSalary);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public decimal? SalaryPoi {
      get {
        if (SalaryIncrease == null)
          return null;

        return Math.Round((decimal)SalaryIncrease / (decimal)PreviousSalary, 2);
      }
    }
  }


}