using Csc.Components.Common;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    public static IQueryable<EmployeeLoanInstallment> Query(this DbQuery<EmployeeLoanInstallment> dbQry) {
      return Query(dbQry, EmployeeLoanInstallmentIncludes.None, null);
    }

    public static IQueryable<EmployeeLoanInstallment> Query(this DbQuery<EmployeeLoanInstallment> dbQry, EmployeeLoanInstallmentIncludes filter) {
      return Query(dbQry, filter, null);
    }

    public static IQueryable<EmployeeLoanInstallment> Query(this DbQuery<EmployeeLoanInstallment> dbQry, EmployeeLoanDataCriteria crit) {
      return Query(dbQry, EmployeeLoanInstallmentIncludes.None, crit);
    }

    public static IQueryable<EmployeeLoanInstallment> Query(this DbQuery<EmployeeLoanInstallment> dbQry, EmployeeLoanInstallmentIncludes filter, EmployeeLoanDataCriteria crit) {
      IQueryable<EmployeeLoanInstallment> qry = null;

      dbQry = dbQry.Include(filter);

      if (crit != null) {
        qry = dbQry.AsQueryable();

        if (crit.StartDate != null && crit.EndDate != null) {
          DateTime startDate = crit.StartDate.Value.ToDay();
          DateTime endDate = crit.EndDate.Value.ToDay();
          qry = qry.Where(s => s.Month >= crit.StartDate && s.Month <= crit.EndDate);
        }

        if (crit.LoanIDs != null) {
          qry = qry.Where(s => crit.LoanIDs.Contains(s.LoanID));
        }

        if (crit.EmployeeIDs != null) {
          qry = qry.Where(s => crit.EmployeeIDs.Contains(s.Loan.EmployeeID));
        }

        if (crit.ListIDs != null) {
          int?[] dbListIds = crit.ListIDs.Select(s => s == 0 ? new int?() : s).ToArray();
          qry = qry.Where(s => dbListIds.Contains(s.Loan.Employee.ListID)).AsQueryable();
        }
      }


      return (qry ?? dbQry).AsQueryable();
    }
  }
}