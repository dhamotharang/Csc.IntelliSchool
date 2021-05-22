using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Charges
    public EmployeeCharge AddOrUpdateCharge(EmployeeCharge item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.ChargeID == 0;

        item = ent.AddOrUpdateItem<EmployeeCharge>(item);

        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.ChargeID.PackArray(),
                new SystemLogDataEntry() { EmployeeID = item.EmployeeID }, null);
        ent.SaveChanges();

        return ent.EmployeeCharges.Query().Where(s => s.ChargeID == item.ChargeID).Single();
      }
    }
    public void DeleteCharge(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeCharge>(id);
      }
    }

    public EmployeeCharge[] GetEmployeeChargesByPeriod(int employeeId, DateTime month, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        return ent.EmployeeCharges
          .Query(EmployeeChargeIncludes.Type, new EmployeeDataCriteria() { EmployeeIDs = employeeId.PackArray() }.SetMonth(month, filter).As<EmployeeDataCriteria>())
          .ToArray();
      }
    }

    public EmployeeCharge[] GetEmployeeCharges(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = CreateModel()) {

        return ent.EmployeeCharges.Query(
          EmployeeChargeIncludes.Type | (includeEmployees ? EmployeeChargeIncludes.Employee : EmployeeChargeIncludes.None),
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


    #region ChargeTypes
    public EmployeeChargeType[] GetChargeTypes() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeChargeType>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeChargeType AddOrUpdateChargeType(EmployeeChargeType item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.TypeID == 0;

        item = ent.AddOrUpdateItem<EmployeeChargeType>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.TypeID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteChargeType(int id) {
      using (var ent = CreateModel()) {
        var linkedItems = ent.EmployeeCharges.Query(new EmployeeDataCriteria() { ItemTypeIDs = id.PackArray() }).ToArray();
        foreach (var itm in linkedItems) {
          itm.TypeID = null;
        }

        ent.RemoveItem<EmployeeChargeType>(id);
      }
    }

    public bool IsChargeTypeUsed(int id) {
      using (var ent = CreateModel()) {
        return ent.EmployeeCharges.Where(s => s.TypeID == id).Count() > 0;
      }
    }
    #endregion

  }

}

