using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.Common {
  public partial interface ICommonDataService {
    #region Nationalities
    [OperationContract][ReferencePreservingDataContractFormat]
    
    Nationality[] GetNationalities();
    [OperationContract][ReferencePreservingDataContractFormat]
    
    Nationality AddOrUpdateNationality(Nationality item);
    [OperationContract][ReferencePreservingDataContractFormat]
    
    void DeleteNationality(int id);
    #endregion

    #region Religion
    [OperationContract][ReferencePreservingDataContractFormat]
    
    Religion[] GetReligions();
    [OperationContract][ReferencePreservingDataContractFormat]
    
    Religion AddOrUpdateReligion(Religion item);
    [OperationContract][ReferencePreservingDataContractFormat]
    
    void DeleteReligion(int id);
    #endregion

  }
}
