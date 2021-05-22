using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region EmployeeAttendance

    public static IQueryable<EmployeeAttendance> Query(this DbQuery<EmployeeAttendance> dbQry) {
      return Query(dbQry, EmployeeAttendanceIncludes.None, null);
    }

    public static IQueryable<EmployeeAttendance> Query(this DbQuery<EmployeeAttendance> dbQry, EmployeeAttendanceIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeAttendance> Query(this DbQuery<EmployeeAttendance> dbQry, EmployeeRangeDataCriteria crit) {
      return Query(dbQry, EmployeeAttendanceIncludes.None, crit);
    }

    public static IQueryable<EmployeeAttendance> Query(this DbQuery<EmployeeAttendance> dbQry,
      EmployeeAttendanceIncludes filter,
      EmployeeRangeDataCriteria crit) {

      IQueryable<EmployeeAttendance> qry = null;

      dbQry = dbQry.Include(filter);

      if (crit != null) {
        qry = dbQry.AsQueryable();

        if (crit.StartDate != null && crit.EndDate != null) {
          DateTime startDate = crit.StartDate.Value.ToDay();
          DateTime endDate = crit.EndDate.Value.ToDay();
          qry = qry.Where(s => s.Date >= startDate && s.Date <= endDate );
        }

        if (crit.EmployeeIDs != null && crit.EmployeeIDs.Count() > 0) {
          qry = qry.Where(s => crit.EmployeeIDs.Contains(s.EmployeeID));
        }
      }

      return (qry ?? dbQry).AsQueryable();
    }
    #endregion
  }
}
