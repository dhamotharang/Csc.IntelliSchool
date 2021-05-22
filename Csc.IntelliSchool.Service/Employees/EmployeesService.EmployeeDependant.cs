using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Dependants
    public EmployeeDependant AddOrUpdateEmployeeDependant(EmployeeDependant item) {
      using (var ent = CreateModel()) {

        bool isInsert = item.DependantID == 0;

        item = ent.AddOrUpdateItem(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(), 
          item.DependantID.PackArray(), 
          new SystemLogDataEntry() { EmployeeID=  item.EmployeeID, Name = item.Person.FullName }, null);
        ent.SaveChanges();

        return item;
      }
    }

    public void DeleteDependant(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeDependant>(id);
      }
    }
    #endregion

  }

}

