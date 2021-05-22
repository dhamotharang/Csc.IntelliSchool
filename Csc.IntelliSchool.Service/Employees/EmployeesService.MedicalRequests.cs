using System;
using System.IO;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    public EmployeeMedicalRequestType[] GetMedicalRequestTypes() {
      using (var ent = CreateModel()) {
        return ent.EmployeeMedicalRequestTypes.ToArray();
      }
    }
  }


}
