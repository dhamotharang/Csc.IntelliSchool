using System.Collections.Generic;

namespace Csc.Wpf {
  public static class FileTypeTranslator {
    public static Dictionary<string, FileType> ExtensionFileTypes = new Dictionary<string, FileType>() {
      { "csv", FileType.CSV },
      { "xlsx", FileType.Excel },
      { "xml", FileType.ExcelML },
      { "xls", FileType.ExcelML },
      { "html", FileType.HTML },
      { "htm", FileType.HTML },
      { "txt", FileType.Text },
      { "png", FileType.Images },
      { "jpeg", FileType.Images },
      { "jpg", FileType.Images },
      { "pdf", FileType.PDF },
    };


    public static Dictionary<FileType, string> FileTypeDialogFilters = new Dictionary<FileType, string>() {
      { FileType.Excel, "Excel Workbook (*.xlsx)|*.xlsx" },
      {FileType.ExcelML, "XML Spreadsheet (*.xml, *.xls)|*.xml},*.xls"},
      {FileType.CSV, "CSV Files (*.csv)|*.csv"},
      {FileType.PDF, "PDF Files (*.pdf)|*.pdf"},
      {FileType.HTML, "HTML Files (*.html, *.htm)|*.html},*.htm"},
      {FileType.Text, "Text Files (*.txt)|*.txt"},
      {FileType.PNG,  "PNG Files (*.png)|*.png"},
      {FileType.JPEG,  "JPEG Files (*.jpeg, *.jpg)|*.jpeg},*.jpg"},
      {FileType.Images, "Image Files (*.jpeg, *.jpg, *.png)|*.jpeg},*.jpg},*.png"},
      {FileType.Any,  "All Files (*.*)|*.*"},
    };
  }
}
