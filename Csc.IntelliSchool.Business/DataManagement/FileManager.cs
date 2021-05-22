using Csc.IntelliSchool.Business.Properties;
using System.IO;

namespace Csc.IntelliSchool.Business {
  public static partial class FileManager {
    //public static string GetRemoteFile(RemoteFileType type, string filename) {
    //  if (string.IsNullOrEmpty(filename))
    //    return null;

    //  return string.Format("{0}/{1}/{2}/{3}", Settings.Default.Service_Base, Settings.Default.Service_Data, type.ToString(), filename);
    //}


    public static string GetLocalFile(LocalFileType type, string file, bool checkExists = false) {
      return null;
    //  if (string.IsNullOrEmpty(file))
    //    return null;

    //  string fullPath = null;

    //  switch (type) {
    //    case LocalFileType.HumanResourcesMedicalTemplates:
    //      fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_HumanResources_MedicalTemplates, file);
    //      break;
    //    case LocalFileType.AppComponents:
    //      fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_Components, file);
    //      break;
    //    default:
    //      fullPath = null;
    //      break;
    //  }

    //  if (fullPath != null && checkExists && File.Exists(fullPath) == false)
    //    fullPath = null;

    //  return fullPath;
    }

  }



}