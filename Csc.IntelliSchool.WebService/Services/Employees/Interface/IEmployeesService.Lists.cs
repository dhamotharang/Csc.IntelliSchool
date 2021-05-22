using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Lists
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeList[] GetLists();
    #endregion
  }

}