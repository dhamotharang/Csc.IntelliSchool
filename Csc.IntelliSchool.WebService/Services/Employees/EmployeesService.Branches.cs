using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Diagnostics;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {


    #region Branches
    public EmployeeBranch[] GetBranches() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeeBranch>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeBranch AddOrUpdateBranch(EmployeeBranch item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.BranchID == 0;

        item = ent.AddOrUpdateItem<EmployeeBranch>(item);


        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.BranchID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();


        return item;
      }
    }
    public void DeleteBranch(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeBranch>(id);
      }
    }
    #endregion
  }

}

