using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region EmployeeBonus

    public static IQueryable<EmployeeBonus> Query(this DbQuery<EmployeeBonus> dbQry) {
      return Query(dbQry, EmployeeBonusIncludes.None, null);
    }

    public static IQueryable<EmployeeBonus> Query(this DbQuery<EmployeeBonus> dbQry, EmployeeBonusIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeBonus> Query(this DbQuery<EmployeeBonus> dbQry, EmployeeDataCriteria crit) {
      return Query(dbQry, EmployeeBonusIncludes.None, crit);
    }

    public static IQueryable<EmployeeBonus> Query(this DbQuery<EmployeeBonus> dbQry, 
      EmployeeBonusIncludes filter , 
      EmployeeDataCriteria crit ) {

      IQueryable<EmployeeBonus> qry = null;

      dbQry = dbQry.Include(filter);

      if (crit != null) {
        if (filter.HasFlag(EmployeeBonusIncludes.Employee))
          dbQry = dbQry.Include(DataExtensions.GetIncludes(EmployeeIncludes.Minimum, "Employee"));

        qry = dbQry.AsQueryable();

        if (crit.ItemIDs != null && crit.ItemIDs.Count() > 0) {
          qry = qry.Where(s => crit.ItemIDs.Contains(s.BonusID));
        }

        if (crit.ItemTypeIDs != null && crit.ItemTypeIDs.Count() >0 && crit.ItemTypeIDs.Contains(0) == false ) {
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