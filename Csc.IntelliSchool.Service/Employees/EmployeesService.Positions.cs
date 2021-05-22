using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Positions
    public EmployeePosition[] GetPositions() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeePosition>("Lists").OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeePosition AddOrUpdatePosition(EmployeePosition item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.PositionID == 0;

        item = ent.AddOrUpdateItem<EmployeePosition>(item);

        ent.Logger.LogDatabase(CurrentUser,
  isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
  item.PositionID.PackArray(),
  new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeletePosition(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeePosition>(id);
      }
    }
    #endregion
  }

}

