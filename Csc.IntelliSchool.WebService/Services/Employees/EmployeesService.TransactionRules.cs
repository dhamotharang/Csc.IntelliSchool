using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region TransactionRules
    public EmployeeTransactionRule[] GetTransactionRules() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeeTransactionRule>().OrderBy(s => s.Type).ThenBy(s=>s.Time).ToArray();
      }
    }
    public EmployeeTransactionRule AddOrUpdateTransactionRule(EmployeeTransactionRule item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.RuleID == 0;

        item = ent.AddOrUpdateItem<EmployeeTransactionRule>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.RuleID.PackArray(),
          null, null);
        ent.SaveChanges();

        return item;

      }
    }
    public void DeleteTransactionRule(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeTransactionRule>(id);
      }
    }
    #endregion


  }

}

