using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Csc.Components.Processors {
  public class ExcelProcessor : IDisposable {
    public IWorkbookFormatProvider FormatProvider { get; private set; }
    public Workbook Workbook { get; private set; }
    public Worksheet ActiveWorksheet { get { return Workbook.ActiveWorksheet; } }

    public ExcelProcessor() {
      FormatProvider = new XlsxFormatProvider();
    }

    public void Dispose() {
      if (ActiveWorksheet != null)
        ActiveWorksheet.Dispose();
      if (Workbook != null)
        Workbook.Dispose();
    }

    public void LoadFile(string filePath) {
      using (Stream input = new FileStream(filePath, FileMode.Open)) {
        Workbook = FormatProvider.Import(input);
      }

      SetActiveSheet(Workbook.Worksheets.FirstOrDefault());
    }

    public void SetActiveSheet(Worksheet sheet) {
      Workbook.ActiveSheet = sheet;
    }

    public CellRange FindRange(TemplateParam startParam, TemplateParam endParam) {
      var startCell = Find(startParam);
      var endCell = Find(endParam);

      if (startCell == null || endCell == null || startCell.CellRanges.First().FromIndex.RowIndex != endCell.CellRanges.First().FromIndex.RowIndex ||
        startCell.CellRanges.First().FromIndex.ColumnIndex > endCell.CellRanges.First().FromIndex.ColumnIndex)
        return null;

      return new CellRange(startCell.CellRanges.First().FromIndex, endCell.CellRanges.First().FromIndex);
    }

    public void FillRange(CellRange range, TemplateParamCollection parameters) {
      FillRange(range, new TemplateParamCollection[] { parameters });
    }
    public void FillRange(CellRange range, IEnumerable<TemplateParamCollection> paramLists) {
      var paramStrings = paramLists.SelectMany(s => s.Select(x => x.Parameter.ToLower())).Distinct().ToArray();
      var origCell = ActiveWorksheet.Cells[range];
      var originalRowIdx = range.FromIndex.RowIndex;

      var rowIdx = originalRowIdx + 1;

      if (ActiveWorksheet.Rows.CanInsert(rowIdx, paramLists.Count()))
        ActiveWorksheet.Rows.Insert(rowIdx, paramLists.Count());


      foreach (var lst in paramLists) {
        for (int colIdx = range.FromIndex.ColumnIndex; colIdx <= range.ToIndex.ColumnIndex; colIdx++) {
          var origValue = ActiveWorksheet.Cells[originalRowIdx, colIdx].GetValue().Value.RawValue.ToLower();
          var cell = ActiveWorksheet.Cells[rowIdx, colIdx];

          var param = lst.Where(s => origValue.Contains(s.Parameter)).FirstOrDefault();
          if (param == null)
            continue;

          SetValue(cell, param.Value);
        }
        rowIdx++;
      }

      ActiveWorksheet.Rows.Remove(originalRowIdx);
    }

    public void ExportFile(string targetFile) {
      using (Stream output = new FileStream(targetFile, FileMode.Create)) {
        FormatProvider.Export(Workbook, output);
      }
    }

    public Stream ExportStream() {
      MemoryStream stm = new MemoryStream();
      FormatProvider.Export(Workbook, stm);
      stm.Flush();
      stm.Position = 0;
      return stm;
    }


    #region Value Change
    public void ClearValue(CellIndex idx, TemplateParam param) {
      ClearValue(idx, param.Parameter);
    }
    public void ClearValue(CellIndex idx, string oldValue) {
      ClearValue(ActiveWorksheet.Cells[idx], oldValue);
    }
    public void ClearValue(CellSelection cell, string oldValue) {
      ReplaceValue(cell, oldValue, string.Empty);
    }

    public void SetValue(CellSelection cell, string newValue) {
      cell.SetValueAsText(newValue);
    }
    public void ReplaceValue(CellSelection cell, string oldValue, string newValue) {
      cell.SetValueAsText(cell.GetValue().Value.RawValue.Replace(oldValue, newValue));
    }
    #endregion

    #region Find and Replace
    public void FindAndReplace(TemplateParam param) {
      FindAndReplace(param.Parameter, param.Value);
    }
    public void FindAndReplace(string txt, string replacement) {

      while (true) {
        var sel = Find(txt);
        if (sel != null)
          SetValue(sel, replacement);
        else
          return;
      }
    }
    public CellSelection Find(TemplateParam param) {
      return Find(param.Parameter);
    }
    public CellSelection Find(string txt) {
      var findOptions = new FindOptions();
      findOptions.StartCell = new WorksheetCellIndex(ActiveWorksheet, 0, 0);
      findOptions.FindBy = FindBy.Rows;
      findOptions.FindIn = FindInContentType.Values;
      findOptions.FindWhat = txt;
      findOptions.MatchCase = false;
      findOptions.FindWithin = FindWithin.Sheet;

      var result = ActiveWorksheet.Find(findOptions);

      if (result == null)
        return null;

      return ActiveWorksheet.Cells[result.FoundCell.CellIndex];
    }
    #endregion
  }
}