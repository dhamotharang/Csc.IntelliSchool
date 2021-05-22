using Csc.IntelliSchool.Data;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.Account {
  [ServiceContract]
  public interface IAccountService {
    [OperationContract][ReferencePreservingDataContractFormat]
    
    User Login(string username, string password);
  }
}
