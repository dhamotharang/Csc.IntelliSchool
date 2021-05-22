using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Allowances
    public EmployeeAllowance AddOrUpdateAllowance(EmployeeAllowance item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.AllowanceID == 0;

        item = ent.AddOrUpdateItem<EmployeeAllowance>(item);

        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.AllowanceID.PackArray(),
                new SystemLogDataEntry() { EmployeeID = item.EmployeeID }, null);
        ent.SaveChanges();

        return ent.EmployeeAllowances.Query().Where(s => s.AllowanceID == item.AllowanceID).Single();
      }
    }
    public void DeleteAllowance(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeAllowance>(id);
      }
    }

    public EmployeeAllowance[] GetEmployeeAllowancesByPeriod(int employeeId, DateTime month, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        return ent.EmployeeAllowances
          .Query(EmployeeAllowanceIncludes.Type, new EmployeeDataCriteria() { EmployeeIDs = employeeId.PackArray() }.SetMonth(month, filter).As<EmployeeDataCriteria>())
          .ToArray();
      }
    }

    public EmployeeAllowance[] GetEmployeeAllowances(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = CreateModel()) {

        return ent.EmployeeAllowances.Query(
          EmployeeAllowanceIncludes.Type | (includeEmployees ? EmployeeAllowanceIncludes.Employee : EmployeeAllowanceIncludes.None),
          new EmployeeDataCriteria() {
            StartDate = startDate,
            EndDate = endDate,
            EmployeeIDs = employeeIds,
            ListIDs = listIds,
            ItemTypeIDs = typeIds,
          }).ToArray();
      }
    }

    #endregion


    #region AllowanceTypes
    public EmployeeAllowanceType[] GetAllowanceTypes() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeAllowanceType>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeAllowanceType AddOrUpdateAllowanceType(EmployeeAllowanceType item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.TypeID == 0;

        item = ent.AddOrUpdateItem<EmployeeAllowanceType>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.TypeID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteAllowanceType(int id) {
      using (var ent = CreateModel()) {
        var linkedItems = ent.EmployeeAllowances.Query(new EmployeeDataCriteria() { ItemTypeIDs = id.PackArray() }).ToArray();
        foreach (var itm in linkedItems) {
          itm.TypeID = null;
        }

        ent.RemoveItem<EmployeeAllowanceType>(id);
      }
    }

    public bool IsAllowanceTypeUsed(int id) {
      using (var ent = CreateModel()) {
        return ent.EmployeeAllowances.Where(s => s.TypeID == id).Count() > 0;
      }
    }
    #endregion

  }

}

