using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Vacations
    public EmployeeVacation AddOrUpdateVacation(EmployeeVacation item) {
      using (var ent = ServiceManager.CreateModel()) {

        bool isInsert = item.VacationID == 0;

        item = ent.AddOrUpdateItem<EmployeeVacation>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.VacationID.PackArray(),
          new SystemLogDataEntry() {
            EmployeeID = item.EmployeeID
          }, null);

        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteVacation(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeVacation>(id);
      }
    }

    #endregion
  }

}

