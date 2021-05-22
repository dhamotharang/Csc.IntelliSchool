using Csc.IntelliSchool.Sync.NewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Csc.IntelliSchool.Sync.OldModel {

  public partial class salaries_monthly_facts  {
    public DateTime Date {
      get {
        return new DateTime(year, month, 1);
      }
    }
  }
}
