using Csc.Components.Common;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Csc.IntelliSchool.Data {
  public partial class DataEntities {
    public virtual void EmployeeDeleteDuplicateAttendance(System.DateTime startDate, System.DateTime endDate, IntList employeeIds) {
      var startDateParam = new SqlParameter("startDate", SqlDbType.Date);
      startDateParam.Value = startDate;

      var endDateParam = new SqlParameter("endDate", SqlDbType.Date);
      endDateParam.Value = endDate;

      var employeeIdsParam = new SqlParameter("employeeIDList", SqlDbType.Structured);
      employeeIdsParam.Value = employeeIds;
      employeeIdsParam.TypeName = employeeIds.GetType().Name;

      this.Database.ExecuteSqlCommand("exec [HR].[EmployeeDeleteDuplicateAttendance] @startDate, @endDate, @employeeIDList;",
        startDateParam, endDateParam, employeeIdsParam);
    }

    public virtual void EmployeeDeleteDuplicateEarnings(System.DateTime startDate, System.DateTime endDate, IntList employeeIds) {
      var startDateParam = new SqlParameter("startDate", SqlDbType.Date);
      startDateParam.Value = startDate;

      var endDateParam = new SqlParameter("endDate", SqlDbType.Date);
      endDateParam.Value = endDate;

      var employeeIdsParam = new SqlParameter("employeeIDList", SqlDbType.Structured);
      employeeIdsParam.Value = employeeIds;
      employeeIdsParam.TypeName = employeeIds.GetType().Name;

      this.Database.ExecuteSqlCommand("exec [HR].[EmployeeDeleteDuplicateEarnings] @startDate, @endDate, @employeeIDList;",
        startDateParam, endDateParam, employeeIdsParam);
    }
  }
}