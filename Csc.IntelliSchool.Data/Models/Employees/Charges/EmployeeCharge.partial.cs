using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Csc.Components.Common;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeCharge {

    [IgnoreDataMember]
    [NotMapped]
    public int? MonthCount {
      get {
        if (StartMonth == null || EndMonth == null || EndMonth < StartMonth)
          return null;

        return DateTimeExtensions.CalculatePeriod(StartMonth.Value, EndMonth.Value).Months + 1;
      }
    }


    public static EmployeeCharge CreateObject(int employeeId) {
      var itm = new EmployeeCharge();
      itm.EmployeeID = employeeId;
      itm.StartMonth = itm.EndMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
      return itm;
    }
  }
}
