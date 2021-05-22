using Csc.IntelliSchool.WebService.Services.Model;
using System.IO;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.FileHandler {
  [ServiceContract]
  public interface IFileHandlerService {
    [OperationContract][ReferencePreservingDataContractFormat]
    RemoteFile UploadFile(RemoteFile sourceFile);
  }
}
