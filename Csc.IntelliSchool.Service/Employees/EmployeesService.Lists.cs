using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Lists
    public EmployeeList[] GetLists() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeList>().OrderBy(s => s.Name).ToArray();
      }
    }
    #endregion
  }

}

