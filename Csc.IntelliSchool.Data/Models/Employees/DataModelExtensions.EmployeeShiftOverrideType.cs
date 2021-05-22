using Csc.Components.Common;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region EmployeeShiftOverrides
    public static IQueryable<EmployeeShiftOverride> Query(this DbQuery<EmployeeShiftOverride> dbQry) {
      return Query(dbQry, EmployeeShiftOverrideIncludes.None, null);
    }

    public static IQueryable<EmployeeShiftOverride> Query(this DbQuery<EmployeeShiftOverride> dbQry, EmployeeShiftOverrideIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeShiftOverride> Query(this DbQuery<EmployeeShiftOverride> dbQry, EmployeeShiftOverrideDataCriteria crit) {
      return Query(dbQry, EmployeeShiftOverrideIncludes.None, crit);
    }

    public static IQueryable<EmployeeShiftOverride> Query(this DbQuery<EmployeeShiftOverride> dbQry,
      EmployeeShiftOverrideIncludes filter,
      EmployeeShiftOverrideDataCriteria crit) {
      // overlapping
      // (StartDate1 <= EndDate2) and (StartDate2 <= EndDate1)

      var qry = dbQry.Include(filter).AsQueryable();

      if (crit != null) {
        if (crit.StartDate != null && crit.EndDate != null) {
          DateTime startDate = crit.StartDate.Value.ToDay();
          DateTime endDate = crit.EndDate.Value.ToDay();
          qry = qry.Where(s => startDate <= s.EndDate && s.StartDate <= endDate);
        }

        if (crit.ShiftIDs != null && crit.ShiftIDs.Count() > 0)
          qry = qry.Where(s => crit.ShiftIDs.Contains(s.ShiftID));


        if (crit.TypeIDs != null && crit.TypeIDs.Count() > 0 && crit.TypeIDs.Contains(0) == false)
          qry = qry.Where(s => s.TypeID != null && crit.TypeIDs.Contains(s.TypeID.Value));


        if (crit.ItemIDs != null && crit.ItemIDs.Count() > 0)
          qry = qry.Where(s => crit.ItemIDs.Contains(s.OverrideID));

        if (crit.Active != null)
          qry = qry.Where(s => s.IsActive == crit.Active);
      }

      return qry;
    }


    public static IOrderedQueryable<EmployeeShiftOverride> OrderLogically(this IQueryable<EmployeeShiftOverride> qry) {
      return qry.OrderBy(s => s.StartDate).ThenBy(s => s.EndDate).ThenBy(s => s.Order).ThenBy(s => s.OverrideID);
    }
    #endregion
  }
}