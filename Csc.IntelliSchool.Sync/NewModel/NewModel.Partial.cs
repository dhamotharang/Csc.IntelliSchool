using Csc.IntelliSchool.Sync.NewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Csc.IntelliSchool.Sync.NewModel {

  public partial class Employee  {
    public DateTime HireMonth {
      get {
        return new DateTime(HireDate.Year , HireDate.Month, 1);
      }
    }
  }
}
