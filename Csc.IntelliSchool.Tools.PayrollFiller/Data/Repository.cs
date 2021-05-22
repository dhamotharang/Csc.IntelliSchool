using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Csc.IntelliSchool.Tools.PayrollFiller.Data {
  public static class Repository {
    private static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[nameof (DataContext)].ConnectionString;
    private static DataContext CreateContext() {
      return new DataContext(ConnectionString) {
        };
    }


    public static EmployeeEarning[] GetEarnings(int[] employeeIds, DateTime month) {
      using (var ctx = CreateContext()) {
        return ctx.EmployeeEarnings.Where(a => employeeIds.Contains(a.EmployeeID) && a.Month == month).ToArray();
      }
    }


  }
}
