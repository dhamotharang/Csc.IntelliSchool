using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region Employee

    public static IQueryable<Employee> Query(this DbQuery<Employee> dbQry) {
      return Query(dbQry, EmployeeIncludes.None, null);
    }

    public static IQueryable<Employee> Query(this DbQuery<Employee> dbQry, EmployeeIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<Employee> Query(this DbQuery<Employee> dbQry, EmployeeDataCriteria crit) {
      return Query(dbQry, EmployeeIncludes.None, crit);
    }


    public static IQueryable<Employee> Query(this DbQuery<Employee> dbQry, EmployeeIncludes filter , 
      EmployeeDataCriteria crit ) {
      var qry = dbQry.Include(filter).AsQueryable();

      if (crit != null) {
        if (crit.EmployeeIDs != null && crit.EmployeeIDs.Count() > 0)
          qry = qry.Where(s => crit.EmployeeIDs.Contains(s.EmployeeID)).AsQueryable();

        if (crit.ListIDs != null && crit.ListIDs.Count() > 0 ) {
          int?[] dbListIds = crit.ListIDs.Select(s => s == 0 ? new int?() : s).ToArray();
          qry = qry.Where(s => dbListIds.Contains(s.ListID)).AsQueryable();
        }

        if (crit.BranchIDs != null && crit.BranchIDs.Count() > 0 && crit.BranchIDs.Contains(0) == false) 
          qry = qry.Where(s => s.BranchID != null && crit.BranchIDs.Contains(s.BranchID.Value));

        if (crit.DepartmentIDs != null && crit.DepartmentIDs.Count() > 0 && crit.DepartmentIDs.Contains(0) == false)
          qry = qry.Where(s => s.DepartmentID != null && crit.DepartmentIDs.Contains(s.DepartmentID.Value));

        if (crit.PositionIDs != null && crit.PositionIDs.Count() > 0 && crit.PositionIDs.Contains(0) == false)
          qry = qry.Where(s => s.PositionID != null && crit.PositionIDs.Contains(s.PositionID.Value));

        if (crit.StartDate != null) 
          qry = qry.Where(s => s.TerminationDate == null || (s.TerminationDate >= crit.StartDate && (s.TerminationHide == false || s.TerminationHide == null))).AsQueryable();
        if (crit.EndDate != null) 
          qry = qry.Where(s => s.HireDate <= crit.EndDate);
      }

      return qry;
    }
    #endregion
    
  }
}