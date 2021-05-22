using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Shifts
    public EmployeeShift[] GetShifts() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeShift>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeShift AddOrUpdateShift(EmployeeShift item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.ShiftID == 0;

        item = ent.AddOrUpdateItem<EmployeeShift>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.ShiftID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteShift(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeShift>(id);
      }
    }
    #endregion

    #region ShiftOverrides
    public EmployeeShiftOverride[] GetShiftOverrides(int? shiftId, int[] typeIds, DateTime? startDate, DateTime? endDate) {
      using (var ent = CreateModel()) {
        return ent.EmployeeShiftOverrides.Query(EmployeeShiftOverrideIncludes.All, new EmployeeShiftOverrideDataCriteria() {
          ShiftIDs = shiftId != null ? shiftId.Value.PackArray() : new int[] { },
          StartDate = startDate,
          EndDate = endDate,
          TypeIDs = typeIds
        }).OrderLogically().ToArray();
      }
    }
    public EmployeeShiftOverride AddOrUpdateShiftOverride(EmployeeShiftOverride item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.OverrideID == 0;

        item = ent.AddOrUpdateItem<EmployeeShiftOverride>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.OverrideID.PackArray(),
          new SystemLogDataEntry() { ParentID = item.ShiftID }, null);
        ent.SaveChanges();

        return ent.EmployeeShiftOverrides.Query(EmployeeShiftOverrideIncludes.All, new EmployeeShiftOverrideDataCriteria() {
          ItemIDs = item.OverrideID.PackArray()
        }).Single();
      }
    }
    public void DeleteShiftOverride(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeShiftOverride>(id);
      }
    }
    #endregion



    #region ShiftOverrideTypes
    public EmployeeShiftOverrideType[] GetShiftOverrideTypes() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeShiftOverrideType>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeShiftOverrideType AddOrUpdateShiftOverrideType(EmployeeShiftOverrideType item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.TypeID == 0;

        item = ent.AddOrUpdateItem<EmployeeShiftOverrideType>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.TypeID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteShiftOverrideType(int id) {
      using (var ent = CreateModel()) {
        var linkedItems = ent.EmployeeShiftOverrides.Where(s=>s.TypeID == id).ToArray();
        foreach (var itm in linkedItems) {
          itm.TypeID = null;
        }

        ent.RemoveItem<EmployeeShiftOverrideType>(id);
      }
    }

    public bool IsShiftOverrideTypeUsed(int id) {
      using (var ent = CreateModel()) {
        return ent.EmployeeShiftOverrides.Where(s => s.TypeID == id).Count() > 0;
      }
    }
    #endregion

  }
}