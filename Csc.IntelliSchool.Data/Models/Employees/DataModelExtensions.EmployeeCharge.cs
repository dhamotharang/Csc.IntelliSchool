using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region EmployeeCharge

    public static IQueryable<EmployeeCharge> Query(this DbQuery<EmployeeCharge> dbQry) {
      return Query(dbQry, EmployeeChargeIncludes.None, null);
    }

    public static IQueryable<EmployeeCharge> Query(this DbQuery<EmployeeCharge> dbQry, EmployeeChargeIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeCharge> Query(this DbQuery<EmployeeCharge> dbQry, EmployeeDataCriteria crit) {
      return Query(dbQry, EmployeeChargeIncludes.None, crit);
    }

    public static IQueryable<EmployeeCharge> Query(this DbQuery<EmployeeCharge> dbQry,
      EmployeeChargeIncludes filter,
      EmployeeDataCriteria crit) {

      IQueryable<EmployeeCharge> qry = null;

      dbQry = dbQry.Include(filter);

      if (crit != null) {
        if (filter.HasFlag(EmployeeChargeIncludes.Employee))
          dbQry = dbQry.Include(DataExtensions.GetIncludes(EmployeeIncludes.Minimum, "Employee"));

        qry = dbQry.AsQueryable();

        if (crit.ItemIDs != null && crit.ItemIDs.Count() > 0) {
          qry = qry.Where(s => crit.ItemIDs.Contains(s.ChargeID));
        }

        if (crit.ItemTypeIDs != null && crit.ItemTypeIDs.Count() > 0 && crit.ItemTypeIDs.Contains(0) == false) {
          qry = qry.Where(s => s.TypeID != null && crit.ItemTypeIDs.Contains(s.TypeID.Value));
        }

        if (crit.ListIDs != null) {
          int?[] dbListIds = crit.ListIDs.Select(s => s == 0 ? new int?() : s).ToArray();
          qry = qry.Where(s => dbListIds.Contains(s.Employee.ListID)).AsQueryable();
        }

        if (crit.StartDate == null)
          crit.StartDate = DateTime.MinValue;
        if (crit.EndDate == null)
          crit.EndDate = DateTime.MaxValue;

        DateTime startDate = crit.StartDate.Value.ToDay();
        DateTime endDate = crit.EndDate.Value.ToDay();
        qry = qry.Where(s => (s.EndMonth == null || crit.StartDate <= s.EndMonth) && (s.StartMonth == null || s.StartMonth <= crit.EndDate));


        if (crit.EmployeeIDs != null && crit.EmployeeIDs.Count() > 0) {
          qry = qry.Where(s => crit.EmployeeIDs.Contains(s.EmployeeID));
        }
      }

      return (qry ?? dbQry).AsQueryable();
    }
    #endregion
  }
}
