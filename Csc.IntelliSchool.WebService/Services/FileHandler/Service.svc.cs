using Csc.IntelliSchool.WebService.Properties;
using Csc.IntelliSchool.WebService.Services.Model;
using System;
using System.IO;
using System.Web.Hosting;

namespace Csc.IntelliSchool.WebService.Services.FileHandler {
  public class FileHandlerService : IFileHandlerService {
    public RemoteFile UploadFile(RemoteFile sourceFile) {
      bool isPhoto = sourceFile.Type == RemoteFileType.EmployeeDependantPhoto || sourceFile.Type == RemoteFileType.EmployeePhoto;

      string targetDir = Path.Combine(HostingEnvironment.MapPath(Settings.Default.FileHandler_DataDirectory), sourceFile.Type.ToString());
      if (Directory.Exists(targetDir) == false)
        Directory.CreateDirectory(targetDir);

      string targetFilename = GenerateFilename(targetDir, isPhoto ? ("." + Settings.Default.FileHandler_PhotoDefaultExtension) : Path.GetExtension(sourceFile.Filename));
      string targetFullPath = Path.Combine(targetDir, targetFilename);


      Stream targetStm = null;

      try {
        if (isPhoto)
          targetStm = new MemoryStream((int)sourceFile.Length);
        else
          targetStm = new FileStream(targetFullPath, FileMode.Create, FileAccess.Write, FileShare.None);

        WriteStream(sourceFile.Stream, targetStm);

        if (isPhoto)
          using (var img = Components.Imaging.ImageHandler.ScaleImage(targetStm, Settings.Default.FileHandler_PhotoMaxWidth, Settings.Default.FileHandler_PhotoMaxHeight)) {
            img.Save(targetFullPath , System.Drawing.Imaging.ImageFormat.Png);
          }

      } finally {
        if (targetStm != null)
          targetStm.Close();
      }

      sourceFile.Filename = targetFilename;

      return sourceFile;
    }

    private static void WriteStream(Stream sourceStm, Stream targetStm) {
      byte[] buffer = new byte[Settings.Default.FileHandler_BufferSize];

      int count = 0;
      while ((count = sourceStm.Read(buffer, 0, buffer.Length)) > 0) {
        targetStm.Write(buffer, 0, count);
      }
    }

    private static string GenerateFilename(string baseDir, string ext) {
      string filename;
      while (true) {
        string suggestedFilename = Guid.NewGuid().ToString() + ext;
        string tmpPath = Path.Combine(baseDir, suggestedFilename);

        if (File.Exists(tmpPath) == false) {
          filename = suggestedFilename;
          break;
        }
      }

      return filename;
    }
  }
}
