using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {

    public static IQueryable<EmployeeEarning> Query(this DbQuery<EmployeeEarning> dbQry, EmployeeRangeDataCriteria crit) {
      IQueryable<EmployeeEarning> qry = null;

      if (crit != null) {
        qry = dbQry.AsQueryable();

        if (crit.StartDate != null && crit.EndDate != null) {
          DateTime startDate = crit.StartDate.Value.ToMonth();
          DateTime endDate = crit.EndDate.Value.ToMonthEnd();
          qry = qry.Where(s => s.Month >= startDate && s.Month <= endDate );
        }

        if (crit.EmployeeIDs != null) {
          qry = qry.Where(s => crit.EmployeeIDs.Contains(s.EmployeeID));
        }
      }

      return (qry ?? dbQry).AsQueryable();
    }
  }
}