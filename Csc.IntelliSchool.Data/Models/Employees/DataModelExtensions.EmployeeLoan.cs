using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region EmployeeLoan

    public static IQueryable<EmployeeLoan> Query(this DbQuery<EmployeeLoan> dbQry) {
      return Query(dbQry, EmployeeLoanIncludes.None, null);
    }

    public static IQueryable<EmployeeLoan> Query(this DbQuery<EmployeeLoan> dbQry, EmployeeLoanIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeLoan> Query(this DbQuery<EmployeeLoan> dbQry, EmployeeLoanDataCriteria crit) {
      return Query(dbQry, EmployeeLoanIncludes.None, crit);
    }

    public static IQueryable<EmployeeLoan> Query(this DbQuery<EmployeeLoan> dbQry, EmployeeLoanIncludes filter, EmployeeLoanDataCriteria crit) {
      IQueryable<EmployeeLoan> qry = null;

      dbQry = dbQry.Include(filter);

      if (crit != null) {
        if (filter.HasFlag(EmployeeLoanIncludes.Employee))
          dbQry = dbQry.Include(DataExtensions.GetIncludes(EmployeeIncludes.Minimum, "Employee"));

        qry = dbQry.AsQueryable();

        if (crit.StartDate != null && crit.EndDate != null) {
          DateTime startDate = crit.StartDate.Value.ToDay();
          DateTime endDate = crit.EndDate.Value.ToDay();
          qry = qry.Where(s => crit.StartDate <= s.EndMonth && s.StartMonth <= crit.EndDate);
        }

        if (crit.LoanIDs != null) {
          qry = qry.Where(s => crit.LoanIDs.Contains(s.LoanID));
        }

        if (crit.EmployeeIDs != null) {
          qry = qry.Where(s => crit.EmployeeIDs.Contains(s.EmployeeID));
        }

        if (crit.ListIDs != null) {
          int?[] dbListIds = crit.ListIDs.Select(s => s == 0 ? new int?() : s).ToArray();
          qry = qry.Where(s => dbListIds.Contains(s.Employee.ListID)).AsQueryable();
        }
      }


      return (qry ?? dbQry).AsQueryable();
    }
    #endregion
  }
}