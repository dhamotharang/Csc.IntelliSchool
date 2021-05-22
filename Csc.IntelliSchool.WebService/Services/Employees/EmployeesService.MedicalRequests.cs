using System;
using System.IO;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    public EmployeeMedicalRequestType[] GetMedicalRequestTypes() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeMedicalRequestTypes.ToArray();
      }
    }
  }


}
