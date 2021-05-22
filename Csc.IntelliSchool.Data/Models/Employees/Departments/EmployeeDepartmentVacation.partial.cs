
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeDepartmentVacation {
    [IgnoreDataMember]
    [NotMapped]
    public string DepartmentNames {
      get {
        if (Departments != null) {
          return Departments.Where(s => s.Department != null).Select(s => s.Department.Name).OrderBy(s => s).Combine(", ");
        }

        return null;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int? DayCount {
      get {
        if (EndDate < StartDate)
          return null;

        var diff = (int)DateTimeExtensions.CalculatePeriod(StartDate, EndDate).Days + 1;

        return diff > 0 ? diff : new int?();
      }
    }

    public IEnumerable<DateTime> GetDays() {
      if (StartDate > EndDate)
        throw new InvalidOperationException();

      DateTime date = StartDate;
      while (date <= EndDate) {
        yield return date;
        date = date.AddDays(1);
      }
    }
  }


}