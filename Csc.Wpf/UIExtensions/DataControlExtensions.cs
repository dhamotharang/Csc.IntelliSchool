using Csc.Components.Common;
using Csc.Wpf.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Csc.Wpf {
  public static partial class DataControlExtensions {
    //#region Search
    //public static void SetSearchText(this RadGridView ctl, string txt) {
    //  var searchPanels = ctl.ChildrenOfType<Telerik.Windows.Controls.GridView.GridViewSearchPanel>().ToArray();
    //    foreach (var searchPanel in searchPanels) {
    //      var viewModel = (searchPanel.DataContext as Telerik.Windows.Controls.GridView.SearchPanel.SearchViewModel);
    //      if (viewModel != null)
    //        viewModel.SearchText = txt;
    //    }
    //}
    //#endregion

    #region Items
    public static void SelectItem(this RadGridView grid, object item) {
      grid.SelectedItem = item;
      grid.ScrollIntoView(item);
    }

    public static T SelectedItemAs<T>(this DataControl ctl) where T : class {
      return ctl.SelectedItem as T;
    }
    public static T[] SelectedItemsAs<T>(this DataControl ctl) {
      return ctl.SelectedItems.Select(s => (T)s).ToArray();
    }

    public static object[] CheckedItems(this RadTreeView ctl) {
      return ctl.Items().Where(s => s.CheckState == System.Windows.Automation.ToggleState.On).Select(s => s.Item).ToArray();
    }

    public static T[] CheckedItemsAs<T>(this RadTreeView ctl) where T : class {
      return CheckedItems(ctl).Select(s => s as T).ToArray();
    }

    public static GridViewRow RowFromItem(this RadGridView ctl, object item) {
      return ctl.ItemContainerGenerator.ContainerFromItem(item) as GridViewRow;
    }
    public static IEnumerable<RadTreeViewItem> Items(this RadTreeView ctl) { return ctl.ItemContainerGenerator.ItemsAs<RadTreeViewItem>(); }
    public static IEnumerable<RadMenuItem> Items(this RadContextMenu ctl) { return ctl.Items.OfType<RadMenuItem>(); }
    public static IEnumerable<T> ItemsAs<T>(this ItemsControl ctl) where T : DependencyObject { return ctl.ItemContainerGenerator.ItemsAs<T>(); }
    ///// <summary>
    ///// Returns items as TreeViewItem
    ///// </summary>
    public static IEnumerable<T> ItemsAs<T>(this ItemContainerGenerator gen) where T : DependencyObject {
      foreach (var itm in gen.Items) {
        yield return (T)gen.ContainerFromItem(itm);
      }
    }

    ///// <summary>
    ///// Returns items as TreeViewItem. Loops through hierarchies
    ///// </summary>
    //public static IEnumerable<T> ItemsLoopAs<T>(this ItemContainerGenerator gen) where T : DependencyObject {
    //  List<T> items = new List<T>();
    //  foreach (var itm in ItemsAs<T>(gen)) {
    //    if (itm == null)
    //      continue;
    //    items.Add(itm);
    //    items.AddRange(ItemsLoopAs<T>(itm.ItemContainerGenerator));
    //  }

    //  return items.ToArray();
    //}
    #endregion

    #region Filtering
    public static void FilterColumns(this RadGridView grid, string colName, bool visible) {
      foreach (var col in grid.Columns) {
        if (col.ColumnGroupName == colName) {
          col.IsVisible = visible;
          if (col.IsVisible == false)
            col.ColumnFilterDescriptor.Clear();
        }
      }
    }
    #endregion

    #region Parent Rows
    public static void FocusSelect(this GridViewRow row) {
      row.GridViewDataControl.Focus();
      row.IsCurrent = true;
      row.IsSelected = true;
      //row.GridViewDataControl.SelectedItem = row;
      //row.GridViewDataControl.CurrentItem = row;
      row.Focus();
      row.BringIntoView();
    }
    public static object SelectParentRow(this RoutedEventArgs e) {
      var row = e.SourceOfType<Telerik.Windows.Controls.GridView.GridViewRow>();
      row.FocusSelect();

      return row.Item;
    }
    public static T SelectParentRow<T>(this RoutedEventArgs e) where T : class {
      return SelectParentRow(e) as T;
    }

    #endregion

    #region Column Removing
    public static void RemoveColumns(this RadGridView grid, params string[] columnNames) {
      for (int i = grid.Columns.Count - 1; i >= 0; i--) {
        var col = grid.Columns[i];
        if (col.UniqueName != null && columnNames.Contains(col.UniqueName)) {
          grid.Columns.RemoveAt(i);
        }
      }
    }
    public static void RemoveColumnGroups(this RadGridView grid, params string[] groupNames) {
      // TODO: Pause sorting until finish
      for (int i = grid.Columns.Count - 1; i >= 0; i--) {
        var col = grid.Columns[i];
        if (col.ColumnGroupName != null && groupNames.Contains(col.ColumnGroupName )) {
          grid.Columns.RemoveAt(i);
        }
      }

      for (int i = grid.ColumnGroups.Count - 1; i >= 0; i--) {
        var grp = grid.ColumnGroups[i];
        if (groupNames.Contains(grp.Name))
          grid.ColumnGroups.Remove(grp);
      }
    }
    #endregion

    #region Sorting and Grouping
    public static void SortBy(this RadGridView grid, params string[] columns) {
      SortBy(grid, ListSortDirection.Ascending, columns);
    }
    public static void SortByDescending(this RadGridView grid, params string[] columns) {
      SortBy(grid, ListSortDirection.Descending, columns);
    }
    public static void SortBy(this RadGridView grid, System.ComponentModel.ListSortDirection direction, params string[] columns) {
      // TODO: Pause sorting until finish
      foreach (var col in columns) {
        var desc = new Telerik.Windows.Controls.GridView.ColumnSortDescriptor() {
          Column = grid.Columns[col],
          SortDirection = direction
        };
        grid.SortDescriptors.Add(desc);
      }
    }
    public static void SortByMember(this RadGridView grid, params string[] members) {
      // TODO: Pause sorting until finish
      foreach (var mem in members) {
        var desc = new Telerik.Windows.Data.SortDescriptor() {
          Member = mem,
          SortDirection = System.ComponentModel.ListSortDirection.Ascending
        };
        grid.SortDescriptors.Add(desc);
      }
    }
    public static void GroupBy(this RadGridView grid, params string[] columns) {
      // TODO: Pause sorting until finish
      foreach (var col in columns) {
        var desc = new ColumnGroupDescriptor() {
          Column = grid.Columns[col],
          SortDirection = System.ComponentModel.ListSortDirection.Ascending,
        };
        grid.GroupDescriptors.Add(desc);
      }

      grid.ExpandAllGroups();
    }
    #endregion

    #region Null Items
    //public static IEnumerable<T> InsertNullItem<T>(this T[] items, Action<T> callback) where T : new() {
    //  return InsertNullItem((IEnumerable<T>)items, callback);
    //}
    public static IEnumerable<T> InsertNullItem<T>(this IEnumerable<T> items, Action<T> callback) where T : new() {
      var nullItem = new T();
      if (callback != null)
        callback(nullItem);
      return new T[] { nullItem }.Concat(items);
    }
    public static void FillAsyncItems<T>(this Selector ctl, T[] result, Exception error, FrameworkElement busyNotifier) where T : new() {
      FillAsyncItems<T>(ctl, result, error, null, busyNotifier);
    }
    public static void FillAsyncItems<T>(this Selector ctl, T[] result, Exception error, Action<T> nullCallback, FrameworkElement busyNotifier) where T : new() {
      FillAsyncItems<T>(ctl, result, error, nullCallback, default(T), busyNotifier);
    }


    public static void FillAsyncItems<T>(this Selector ctl, T[] result, Exception error, Action<T> nullCallback, T itemToSelect, FrameworkElement busyNotifier) where T : new() {
      if (null == error) {
        ctl.ItemsSource = nullCallback != null ? result.InsertNullItem(nullCallback) : result;

        if (itemToSelect != null)
          ctl.SelectedItem = itemToSelect;
      } else
        Popup.AlertError(error);

      if (busyNotifier != null)
        busyNotifier.ClearBusy();
    }

    //public static void FillAsyncItems<T>(this Selector ctl, T[] result, Exception error, Action<T> nullCallback, T itemToSelect, FrameworkElement busyNotifier) where T : new() {
    //  {

    //  }

    public static void FillItems(this ItemsControl ctl, Type enumType, bool includeEmptyItem) {
      var items = Enum.GetNames(enumType);
      FillItems(ctl, items, includeEmptyItem);
    }

    public static void FillItems(this ItemsControl ctl, string[] items, bool includeEmptyItem) {
      if (includeEmptyItem)
        items = new string[] { string.Empty }.Concat(items).ToArray();

      ctl.ItemsSource = items;
    }
    #endregion

    #region Export
    public static void ExportAsync(this RadGridView grid) {
      ItemsFilterWindow wnd = new ItemsFilterWindow(grid, "Export");
      grid.DisplayModal<ItemsFilterWindow>(wnd, grid, OnGridExportWindow);
    }


    private static void OnGridExportWindow(ItemsFilterWindow wnd, object state) {
      if (wnd.DialogResult != true)
        return;

      var grid = ((RadGridView)state);

      grid.SetBusy();
      grid.ExportAsync(wnd.Items, OnGridExportCompleted);
    }

    private static void OnGridExportCompleted(GridExportAsyncState state, System.Exception error) {
      state.GridView.ClearBusy();
      if (error == null) {
        if (state.Filename != null) {
          Popup.Confirm(null, string.Format(Properties.Resources.Popup_Confirm_Exported, state.Filename), () => {
            try {
              ProcessExtensions.Start(state.Filename);
            } catch (Exception ex) {
              Popup.AlertError(ex);
            }
          });
        }
      } else
        Popup.AlertError(error);
    }


    public static void ExportAsync(this RadGridView grid, IEnumerable items, AsyncState<GridExportAsyncState> callback) {
      try {
        string filename = Popup.SaveFile(FileType.GridExport);
        if (filename == null) {
          Async.OnCallback<GridExportAsyncState>(new GridExportAsyncState(grid), null, callback);
          return;
        }

        //grid.ElementExporting += GridView_ElementExporting;

        var type = Popup.GetFileType(filename);

        if (type.HasFlag(FileType.PDF))
          ExportPDF(grid, items, callback, filename);
        else if (type.HasFlag(FileType.Excel))
          ExportExcel(grid, items, callback, filename);
        else
          ExportOtherFormats(grid, items, callback, filename, type);
      } catch (Exception ex) {
        Async.OnCallback<GridExportAsyncState>(new GridExportAsyncState(grid), ex, callback);
      }
    }

    private static void ExportPDF(RadGridView grid, IEnumerable items, AsyncState<GridExportAsyncState> callback, string filename) {
      var opt = new GridViewPdfExportOptions();
      opt.Items = items;
      opt.ShowColumnHeaders = true;
      opt.ShowColumnFooters = false;
      opt.ShowGroupFooters = false;
      opt.ExportDefaultStyles = true;

      Exception error = null;
      try {
        using (Stream stm = new FileStream(filename, FileMode.Create)) {
          grid.ExportToPdf(stm, opt);
        }
      } catch (Exception ex) {
        error = ex;
      } finally {
        Async.OnCallback(new GridExportAsyncState(grid, filename), error, callback);
      }
    }

    private static void ExportExcel(RadGridView grid, IEnumerable items, AsyncState<GridExportAsyncState> callback, string filename) {
      var opt = new GridViewDocumentExportOptions();
      opt.Items = items;
      opt.ShowColumnHeaders = true;
      opt.ShowColumnFooters = false;
      opt.ShowGroupFooters = false;
      opt.ExportDefaultStyles = true;

      Exception error = null;
      try {
        using (Stream stm = new FileStream(filename, FileMode.Create)) {
          grid.ExportToXlsx(stm, opt);
        }
      } catch (Exception ex) {
        error = ex;
      } finally {
        Async.OnCallback(new GridExportAsyncState(grid, filename), error, callback);
      }
    }



    private static void ExportOtherFormats(RadGridView grid, IEnumerable items, AsyncState<GridExportAsyncState> callback, string filename, FileType type) {
      ExportFormat format = ExportFormat.Csv;
      if (type.HasFlag(FileType.CSV))
        format = ExportFormat.Csv;
      else if (type.HasFlag(FileType.HTML))
        format = ExportFormat.Html;
      else if (type.HasFlag(FileType.ExcelML)) // for better use
        format = ExportFormat.ExcelML;
      else if (type.HasFlag(FileType.Text))
        format = ExportFormat.Text;

      GridViewExportOptions opt = null;

      if (format == ExportFormat.Csv)
        opt = new GridViewCsvExportOptions() {
          UseSystemCultureSeparator = true
        };
      else
        opt = new GridViewExportOptions();

      opt.Format = format;
      opt.Encoding = System.Text.Encoding.UTF8;
      opt.Items = items;
      opt.ShowColumnHeaders = true;
      opt.ShowColumnFooters = false;
      opt.ShowGroupFooters = false;

      Stream stm = new FileStream(filename, FileMode.Create);
      grid.ExportAsync(stm, () => {
        Async.OnCallback(new GridExportAsyncState(grid, filename), null, callback);
      }, opt, true);
    }


    #endregion

    #region Columns
    public static void SetColumnExpression(this RadGridView grid, string col, System.Linq.Expressions.Expression expression) {
      GridViewExpressionColumn column = grid.Columns[col] as GridViewExpressionColumn;
      column.Expression = expression;
    }
    #endregion

    #region Other
    public static bool HasHeader(this Telerik.Windows.Controls.GridViewColumn col) {
      return col.Header != null && col.Header.ToString().Trim().Length > 0;
    }

    public static bool HasHeader(this Telerik.Windows.Controls.GridViewColumnGroup col) {
      return col.Header != null && col.Header.ToString().Trim().Length > 0;
    }

    #endregion
  }

  public class GridExportAsyncState {
    public RadGridView GridView { get; set; }
    public string Filename { get; set; }


    public GridExportAsyncState() { }
    public GridExportAsyncState(RadGridView grid) : this(grid, null) { }
    public GridExportAsyncState(RadGridView grid, string filename) { this.GridView = grid; this.Filename = filename; }

  }
}