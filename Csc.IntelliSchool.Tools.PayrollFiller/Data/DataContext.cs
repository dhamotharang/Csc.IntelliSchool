using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Csc.IntelliSchool.Tools.PayrollFiller.Data {
  public class DataContext : DbContext {
    public DbSet<EmployeeEarning> EmployeeEarnings { get; set; }

    public DataContext(string connStr) : base(connStr) {

    }
  }
}
