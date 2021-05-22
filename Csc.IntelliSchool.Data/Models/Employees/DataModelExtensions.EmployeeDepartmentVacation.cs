using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region EmployeeDepartmentVacations
    //public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry, int departmentId, DateTime? month = null, PeriodFilter filter = PeriodFilter.None) {
    //  return Query(dbQry, new int[] { departmentId }, month, filter);
    //}

    //public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry, int[] departmentIds = null, DateTime? month = null, PeriodFilter filter = PeriodFilter.None) {
    //  EmployeeDepartmentVacationDataCriteria crit = new Data.EmployeeDepartmentVacationDataCriteria();
    //  crit.DepartmentIDs = departmentIds;

    //  if (month != null && filter != PeriodFilter.None) {
    //    DateTime startDate, endDate;
    //    DateTimeExtensions.GetMonthOverlapFilterRange(month.Value, filter, out startDate, out endDate);

    //    crit.StartDate = startDate;
    //    crit.EndDate = endDate;
    //  }

    //  return Query(dbQry, crit);

    //}

    ////public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry) {
    ////  return Query(dbQry, null, null, null);
    ////}
    //public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry, EmployeeDepartmentVacationDataCriteria crit) {
    //  return Query(dbQry, EmployeeDepartmentVacationDataFilter.None, crit);
    //}


    public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry) {
      return Query(dbQry, EmployeeDepartmentVacationIncludes.None, null);
    }

    public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry, EmployeeDepartmentVacationIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry, EmployeeDepartmentVacationDataCriteria crit) {
      return Query(dbQry, EmployeeDepartmentVacationIncludes.None, crit);
    }

    public static IQueryable<EmployeeDepartmentVacation> Query(this DbQuery<EmployeeDepartmentVacation> dbQry, 
      EmployeeDepartmentVacationIncludes filter,
      EmployeeDepartmentVacationDataCriteria crit) {
      // overlapping
      // (StartDate1 <= EndDate2) and (StartDate2 <= EndDate1)

      var qry = dbQry.Include(filter).AsQueryable();

      if (crit != null) {
        if (crit.StartDate != null && crit.EndDate != null) {
          DateTime startDate = crit.StartDate.Value.ToDay();
          DateTime endDate = crit.EndDate.Value.ToDay();
          qry = qry.Where(s => startDate <= s.EndDate && s.StartDate <= endDate);
        }

        if (crit.DepartmentIDs != null && crit.DepartmentIDs.Count() > 0) {
          qry = qry.Where(s => s.Departments.Where(x => crit.DepartmentIDs.Contains(x.DepartmentID)).Count() > 0);
        }
      }

      return qry;
    }
    #endregion
  }
}