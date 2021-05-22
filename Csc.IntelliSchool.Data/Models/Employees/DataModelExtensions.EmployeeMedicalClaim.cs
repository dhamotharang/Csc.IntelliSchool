using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region Medical Claims
    //public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry) {
    //  return Query(dbQry, null, null, null, null);
    //}
    //public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry, int[] claimIds) {
    //  return Query(dbQry, claimIds, null, null, null);
    //}
    //public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry, int[] statusIds, DateTime? startDate, DateTime? endDate) {
    //  return Query(dbQry, null, statusIds, startDate, endDate);
    //}


    public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry) {
      return Query(dbQry, EmployeeMedicalClaimIncludes.None, null);
    }

    public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry, EmployeeMedicalClaimIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry, EmployeeMedicalClaimDataCriteria crit) {
      return Query(dbQry, EmployeeMedicalClaimIncludes.None, crit);
    }



    public static IQueryable<EmployeeMedicalClaim> Query(this DbQuery<EmployeeMedicalClaim> dbQry, EmployeeMedicalClaimIncludes filter, EmployeeMedicalClaimDataCriteria crit) {
      var qry = dbQry.Include(filter).AsQueryable();

      if (crit.ClaimIDs != null)
        qry = qry.Where(s => crit.ClaimIDs.Contains(s.ClaimID)).AsQueryable();

      if (crit.StatusIDs != null)
        qry = qry.Where(s => s.StatusID == null || crit.StatusIDs.Contains(s.StatusID.Value));


      crit.StartDate = crit.StartDate.ToDay();
      crit.EndDate = crit.EndDate.ToDayEnd();

      if (crit.StartDate != null && crit.EndDate != null)
        qry = qry.Where(s => s.Date >= crit.StartDate && s.Date <= crit.EndDate);

      return qry.AsQueryable();
    }
    #endregion
  }
}