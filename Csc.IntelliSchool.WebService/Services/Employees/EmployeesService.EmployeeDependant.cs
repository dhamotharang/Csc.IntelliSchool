using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Dependants
    public EmployeeDependant AddOrUpdateEmployeeDependant(EmployeeDependant item) {
      using (var ent = ServiceManager.CreateModel()) {

        bool isInsert = item.DependantID == 0;

        item = ent.AddOrUpdateItem(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(), 
          item.DependantID.PackArray(), 
          new SystemLogDataEntry() { EmployeeID=  item.EmployeeID, Name = item.FullName }, null);
        ent.SaveChanges();

        return item;
      }
    }

    public void DeleteDependant(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeDependant>(id);
      }
    }
    #endregion

  }

}

