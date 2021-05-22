using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Csc.IntelliSchool.Tools.PayrollFiller.Data {
  [Table("EmployeeEarningsView")]
  public class EmployeeEarning {
    [Key]
    public int EarningID { get; set; }
    public int EmployeeID { get; set; }
    public DateTime Month { get; set; }
    public int Net { get; set; }

  }
}
