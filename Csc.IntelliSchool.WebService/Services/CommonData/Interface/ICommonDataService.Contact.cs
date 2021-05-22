using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Csc.IntelliSchool.WebService.Services.Common {
  public partial interface ICommonDataService {
    [OperationContract][ReferencePreservingDataContractFormat]
    string[] GetContactReferences();
  }
}
