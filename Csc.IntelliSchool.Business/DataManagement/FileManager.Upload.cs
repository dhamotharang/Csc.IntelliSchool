using Csc.Components.Common;
using System.IO;

namespace Csc.IntelliSchool.Business {
  public static partial class FileManager {
    public static void UploadEmployeePhoto(string fullPath, AsyncState<string> callback) {
      //Async.AsyncCall(() => UploadFileSync(fullPath, RemoteFileType.EmployeePhoto), callback);
    }

    public static void UploadEmployeeDocument(string fullPath, AsyncState<string> callback) {
      //Async.AsyncCall(() => UploadFileSync(fullPath, RemoteFileType.EmployeeDocument), callback);
    }

//    internal static string  UploadFileSync(string fullPath, RemoteFileType fileType) {
//      string filename = Path.GetFileName(fullPath);
//      var stream = (Stream)File.OpenRead(fullPath);
//      var length = stream.Length;
//      RemoteFileType type = fileType;

//      try {
//#warning please complete
//        //Service..UploadFile(ref filename, ref length, ref type, ref stream);
//        return filename;
//      }finally {
//        stream.Dispose();
//      }
//    }
  }
}