using System.IO;

namespace Csc.IntelliSchool.Data {
  public static class AppPath {
    public static string FormatPath(AppPathSection section, string file, bool checkExists = false) {
      if (string.IsNullOrEmpty(file))
        return null;

      string fullPath = null;
      
      switch (section) {
        case AppPathSection.HumanResourcesDocuments:
          //fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_HumanResources_Documents, file);
          break;
        case AppPathSection.HumanResourcesPhotos:
          //fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_HumanResources_Photos, file);
          break;
        case AppPathSection.HumanResourcesFiles:
          //fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_HumanResources_Files, file);
          break;
        case AppPathSection.HumanResourcesMedicalTemplates:
          //fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_HumanResources_MedicalTemplates, file);
          break;
        case AppPathSection.AppComponents:
          //fullPath = Path.Combine(Settings.Default.Path_Root, Settings.Default.Path_Components, file);
          break;
        default:
          fullPath = null;
          break;
      }

      if (fullPath != null && checkExists && File.Exists(fullPath) == false)
        fullPath = null;

      return fullPath;
    }
  }

  public enum AppPathSection {
    HumanResourcesDocuments,
    HumanResourcesPhotos,
    HumanResourcesFiles,
    HumanResourcesMedicalTemplates,
    AppComponents
  }


}