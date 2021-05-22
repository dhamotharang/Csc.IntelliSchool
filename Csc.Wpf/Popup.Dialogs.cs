using System;
using System.Linq;
using System.IO;

namespace Csc.Wpf {
  public static partial class Popup {
    public static FileType GetFileType(string filename) {
      string extension = Path.GetExtension(filename).TrimStart('.').ToLower();

      if (FileTypeTranslator.ExtensionFileTypes.ContainsKey(extension))
        return FileTypeTranslator.ExtensionFileTypes[extension];

      return FileType.Any;
    }
    private static string GenerateDialogFilter(FileType type) {
      string filterString = string.Empty;

      var filters = FileTypeTranslator.FileTypeDialogFilters.Where(s => type.HasFlag(s.Key)).ToArray();

      filterString = string.Join("|", filters.Select(s => s.Value));

      return filterString;
    }


    public static string SelectFile(FileType type) {
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.CheckFileExists = dlg.CheckPathExists = true;
      dlg.DereferenceLinks = true;
      dlg.ValidateNames = true;

      dlg.Filter = GenerateDialogFilter(type);

      return dlg.ShowDialog() == true ? dlg.FileName : null;
    }

    public static string SaveFile(FileType type) {
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
      dlg.OverwritePrompt = true;
      dlg.DereferenceLinks = true;
      dlg.ValidateNames = true;
      dlg.AddExtension = true;
      dlg.Filter = GenerateDialogFilter(type);

      if (dlg.ShowDialog() == true)
        return dlg.FileName;
      else
        return null;
    }
    public static string SaveFile(string filename) {
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
      dlg.OverwritePrompt = true;
      dlg.DereferenceLinks = true;
      dlg.ValidateNames = true;
      dlg.FileName = Path.GetFileName(filename) ?? string.Empty;
      dlg.AddExtension = true;

      var ext = Path.GetExtension(filename).TrimStart('.');
      if (ext.Length > 0)
        dlg.Filter = string.Format(string.Format("{0} Files (*.{0})|*.{0}|All Files (*.*)|*.*", ext));
      else
        dlg.Filter = FileTypeTranslator.FileTypeDialogFilters[FileType.Any];

      if (dlg.ShowDialog() == true) {
        File.Copy(filename, dlg.FileName, true);
        return dlg.FileName;
      } else

        return null;
    }
  }
}