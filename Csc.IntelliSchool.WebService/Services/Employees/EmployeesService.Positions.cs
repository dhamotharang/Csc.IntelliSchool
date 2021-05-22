using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Positions
    public EmployeePosition[] GetPositions() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeePosition>("Lists").OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeePosition AddOrUpdatePosition(EmployeePosition item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.PositionID == 0;

        item = ent.AddOrUpdateItem<EmployeePosition>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
  isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
  item.PositionID.PackArray(),
  new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeletePosition(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeePosition>(id);
      }
    }
    #endregion
  }

}

