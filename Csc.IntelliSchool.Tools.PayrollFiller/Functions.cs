using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace Csc.IntelliSchool.Tools.PayrollFiller {
  public class Functions {
    public static string SelectFile(Window wnd = null, Action<OpenFileDialog> opts = null) {
      OpenFileDialog dlg = new OpenFileDialog();
      if (opts != null) {
        opts(dlg);
      }
      if (true != dlg.ShowDialog(wnd))
        return null;

      return dlg.FileName;
    }


    public static string[] GetWorksheetNames(string filename) {
      List<string> names = null;

      Excel.Application app = new Excel.Application();
      try {
        app.Visible = false;

        Excel.Workbook workbook = app.Workbooks.Open(filename, ReadOnly: true, UpdateLinks: false);

        try {
          names = new List<string>(workbook.Sheets.Count);
          foreach (var obj in workbook.Sheets) {
            var sheet = ((Excel.Worksheet)obj);
            names.Add(sheet.Name);
          }
        } finally {
          workbook.Close(false);
          Marshal.ReleaseComObject(workbook);
        }
      } finally {
        app.Quit();
        Marshal.ReleaseComObject(app);
      }

      return names.ToArray();
    }



  }
}
