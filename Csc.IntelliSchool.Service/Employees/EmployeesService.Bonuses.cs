using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Bonuses
    public EmployeeBonus AddOrUpdateBonus(EmployeeBonus item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.BonusID == 0;

        item =  ent.AddOrUpdateItem<EmployeeBonus>(item);
         
        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.BonusID.PackArray(),
                new SystemLogDataEntry() { EmployeeID = item.EmployeeID }, null);
        ent.SaveChanges();

        return ent.EmployeeBonuses.Query(EmployeeBonusIncludes.Type).Where(s => s.BonusID == item.BonusID).Single();
      }
    }
    public void DeleteBonus(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeBonus>(id);
      }
    }


    public EmployeeBonus[] GetEmployeeBonusesByPeriod(int employeeId, DateTime month, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        return ent.EmployeeBonuses
          .Query(EmployeeBonusIncludes.Type,  
          new EmployeeDataCriteria() { EmployeeIDs = employeeId.PackArray() }.SetMonth(month, filter).As<EmployeeDataCriteria>())
          .ToArray();
      }
    }

    public EmployeeBonus[] GetEmployeeBonuses(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = CreateModel()) {

        return ent.EmployeeBonuses.Query(
          EmployeeBonusIncludes.Type | (includeEmployees ? EmployeeBonusIncludes.Employee : EmployeeBonusIncludes.None),
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


    #region BonusTypes
    public EmployeeBonusType[] GetBonusTypes() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeBonusType>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeBonusType AddOrUpdateBonusType(EmployeeBonusType item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.TypeID == 0;

        item = ent.AddOrUpdateItem<EmployeeBonusType>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.TypeID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteBonusType(int id) {
      using (var ent = CreateModel()) {
        var linkedItems = ent.EmployeeBonuses.Query(new EmployeeDataCriteria() { ItemTypeIDs = id.PackArray() }).ToArray();
        foreach (var itm in linkedItems) {
          itm.TypeID = null;
        }

        ent.RemoveItem<EmployeeBonusType>(id);
      }
    }

    public bool IsBonusTypeUsed(int id) {
      using (var ent = CreateModel()) {
        return ent.EmployeeBonuses.Where(s => s.TypeID == id).Count() > 0;
      }
    }
    #endregion

  }

}

