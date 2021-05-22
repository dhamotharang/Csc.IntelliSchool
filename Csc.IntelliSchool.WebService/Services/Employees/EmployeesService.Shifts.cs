using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Shifts
    public EmployeeShift[] GetShifts() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeeShift>("Overrides").OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeShift AddOrUpdateShift(EmployeeShift item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.ShiftID == 0;

        item = ent.AddOrUpdateItem<EmployeeShift>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.ShiftID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteShift(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeShift>(id);
      }
    }
    #endregion

    #region ShiftOverrides
    public EmployeeShiftOverride[] GetShiftOverrides() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeeShiftOverride>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeShiftOverride AddOrUpdateShiftOverride(EmployeeShiftOverride item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.OverrideID == 0;

        item = ent.AddOrUpdateItem<EmployeeShiftOverride>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert  ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.OverrideID.PackArray(),
          new SystemLogDataEntry() { ParentID = item.ShiftID, Name= item.Name}, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteShiftOverride(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeShiftOverride>(id);
      }
    }
    #endregion
  }

}

