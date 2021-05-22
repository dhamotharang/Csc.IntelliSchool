using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {

    #region EmployeeDeduction
    public static IQueryable<EmployeeDeduction> Query(this DbQuery<EmployeeDeduction> dbQry) {
      return Query(dbQry, EmployeeDeductionIncludes.None, null);
    }

    public static IQueryable<EmployeeDeduction> Query(this DbQuery<EmployeeDeduction> dbQry, EmployeeDeductionIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeDeduction> Query(this DbQuery<EmployeeDeduction> dbQry, EmployeeDataCriteria crit) {
      return Query(dbQry, EmployeeDeductionIncludes.None, crit);
    }



    public static IQueryable<EmployeeDeduction> Query(this DbQuery<EmployeeDeduction> dbQry, EmployeeDeductionIncludes filter = EmployeeDeductionIncludes.None, EmployeeDataCriteria crit = null) {
      IQueryable<EmployeeDeduction> qry = null;

      dbQry = dbQry.Include(filter);

      if (crit != null) {
        if (filter.HasFlag(EmployeeDeductionIncludes.Employee))
          dbQry = dbQry.Include(DataExtensions.GetIncludes(EmployeeIncludes.Minimum, "Employee"));

        qry = dbQry.AsQueryable();

        if (crit.ItemIDs != null && crit.ItemIDs.Count() > 0) {
          qry = qry.Where(s => crit.ItemIDs.Contains(s.DeductionID));
        }

        if (crit.ItemTypeIDs != null && crit.ItemTypeIDs.Count() > 0 && crit.ItemTypeIDs.Contains(0) == false) {
          qry = qry.Where(s => s.TypeID != null && crit.ItemTypeIDs.Contains(s.TypeID.Value));
        }

        if (crit.ListIDs != null) {
          int?[] dbListIds = crit.ListIDs.Select(s => s == 0 ? new int?() : s).ToArray();
          qry = qry.Where(s => dbListIds.Contains(s.Employee.ListID)).AsQueryable();
        }
        
        if (crit.StartDate != null && crit.EndDate != null) {
          qry = qry.Where(s => s.Date >= crit.StartDate && s.Date <= crit.EndDate);
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