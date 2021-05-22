using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Csc.IntelliSchool.WebService.Services.SystemAdmin {
  public partial interface ISystemAdminService {
    [OperationContract][ReferencePreservingDataContractFormat]
    SystemLog[] GetSystemLog(DateTime start, DateTime end);
  }
}
