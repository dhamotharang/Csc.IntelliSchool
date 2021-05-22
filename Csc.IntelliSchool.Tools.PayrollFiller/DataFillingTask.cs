using Microsoft.Office.Interop.Excel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Csc.IntelliSchool.Tools.PayrollFiller {
  public class DataFillingTask {
    private Application Application { get; set; }
    public string Filename { get; private set; }
    public int SheetIndex { get; private set; }
    public DateTime Month { get; private set; }

    public DataFillingTask(string filename, int sheetIdx, DateTime month) {
      this.Filename = filename;
      this.SheetIndex = sheetIdx;
      this.Month = month;
    }

    public void Run() {
      Application = new Application();
      try {
        Application.Visible = false;


        Workbook wb = Application.Workbooks.Open(Filename, ReadOnly: true, UpdateLinks: false);
        try {
          Worksheet sheet = (Worksheet)wb.Sheets[SheetIndex];
          int[] employeeIds = GetEmployeeIds(sheet);

          if (employeeIds.Length == 0)
            return;

          var earnings = Data.Repository.GetEarnings(employeeIds, Month);


          SaveData(sheet, earnings);

        } finally {
          //wb.Close(false);
          Marshal.ReleaseComObject(wb);
        }
      } finally {
        Application.Visible = true;
        //Application.Quit();
        Marshal.ReleaseComObject(Application);
        Application = null;
      }

    }

    private void SaveData(Worksheet sheet, Data.EmployeeEarning[] earnings) {
      Range range = sheet.UsedRange;

      int rowIdx = 0;

      if (range.Rows.Count < (rowIdx + 1)) {
        return;
      }

      while (true) {
        rowIdx++;

        if (rowIdx > sheet.UsedRange.Rows.Count)
          break;

        int id = GetEmployeeIDValue(sheet, rowIdx) ?? 0;
        if (id == 0)
          continue;

        var earn = earnings.FirstOrDefault(a => a.EmployeeID == id);
        if (earn == null)
          continue;


        range.Cells[rowIdx, range.Columns.Count + 1] = earn.Net;
      }
    }

    private int[] GetEmployeeIds(Worksheet sheet) {
      List<int> items = new List<int>(sheet.UsedRange.Rows.Count - 1);

      int rowIdx = 1;

      if (sheet.UsedRange.Count < rowIdx) {
        return items.ToArray();
      }


      while (rowIdx <= sheet.UsedRange.Rows.Count) {
        int id = GetEmployeeIDValue(sheet, rowIdx) ?? 0;
        if (id > 0) {
          items.Add(id);
        }
        rowIdx++;
      }

      return items.ToArray();
    }

    private static int? GetEmployeeIDValue(Worksheet sheet, int rowIdx) {
      var value = sheet.UsedRange.Cells[rowIdx, 1].Value2;

      if (value == null)
        return null;

      var valueStr = value.ToString();

      if (valueStr.Trim().Length == 0)
        return null;

      int tmpVal = 0;
      if (int.TryParse(valueStr.Trim(), out tmpVal))
        return tmpVal;

      return null;
    }
  }
}
