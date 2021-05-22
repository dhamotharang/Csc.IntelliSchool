using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Diagnostics;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {


    #region Branches
    public EmployeeBranch[] GetBranches() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeBranch>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeBranch AddOrUpdateBranch(EmployeeBranch item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.BranchID == 0;

        item = ent.AddOrUpdateItem<EmployeeBranch>(item);


        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.BranchID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();


        return item;
      }
    }
    public void DeleteBranch(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeBranch>(id);
      }
    }
    #endregion
  }

}

