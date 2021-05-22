using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region TransactionRules
    public EmployeeTransactionRule[] GetTransactionRules() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeTransactionRule>().OrderBy(s => s.Type).ThenBy(s=>s.Time).ToArray();
      }
    }
    public EmployeeTransactionRule AddOrUpdateTransactionRule(EmployeeTransactionRule item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.RuleID == 0;

        item = ent.AddOrUpdateItem<EmployeeTransactionRule>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.RuleID.PackArray(),
          null, null);
        ent.SaveChanges();

        return item;

      }
    }
    public void DeleteTransactionRule(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeTransactionRule>(id);
      }
    }
    #endregion


  }

}

