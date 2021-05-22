using Csc.Components.Common.Data;
using Csc.Wpf.Data;
using Csc.Wpf.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Csc.Wpf {
  public static partial class DynamicColumnsExtensions {
    private static BoolConverter _converter;
    private static object _lockObject = new object();

    public static BoolConverter BooleanImageConverter {
      get {
        if (null == _converter)
          lock (_lockObject) {
            if (null == _converter) {
              _converter = new BoolConverter();
              _converter.TrueValue = "/Csc.Wpf;component/Assets/Images/16/true.png";
              _converter.FalseValue = "/Csc.Wpf;component/Assets/Images/16/false.png";
            }
          }

        return _converter;
      }

    }

    public static string GetHeader(this Telerik.Windows.Controls.GridViewColumn col) {
      return col.Header != null ? col.Header.ToString() : string.Empty;
    }

    public static void FormatDynamicColumn(this GridViewAutoGeneratingColumnEventArgs args) {
      var header = args.Column.GetHeader();

      var flags = DynObject.GetColumnFlags(header);

      args.Column.Header = DynObject.GetColumnHeader(header);

      if (flags.HasFlag(DynObjectColumnFlags.Hidden)) {
        args.Cancel = true;
        return;
      }

      if (flags.HasFlag(DynObjectColumnFlags.Boolean))
        ReplaceColumn<GridViewCheckBoxColumn>(args);

      else if (flags.HasFlag(DynObjectColumnFlags.TrueFalse)) {
        ReplaceColumn<GridViewImageColumn>(args);
        var col = (GridViewImageColumn)args.Column;
        col.DataMemberBinding.Converter = BooleanImageConverter;
        if (col.DataMemberBinding != null && col.DataMemberBinding.Path != null)
          col.FilterMemberPath = col.DataMemberBinding.Path .Path;
      } 
      
      else if (flags.HasFlag(DynObjectColumnFlags.Date)) {
        var col = (GridViewDataColumn)args.Column;
        col.DataType = typeof(DateTime);
        col.DataMemberBinding.StringFormat = "d";
        col.HeaderTextAlignment = TextAlignment.Right;
        col.TextAlignment = TextAlignment.Right;
      } 
      
      else if (flags.HasFlag(DynObjectColumnFlags.Text)) {
        var col = (GridViewDataColumn)args.Column;
        col.DataType = typeof(string);
      }

      else if (flags.HasFlag(DynObjectColumnFlags.Number)) {
        var col = (GridViewDataColumn)args.Column;
        col.DataMemberBinding.StringFormat = "N";
        col.HeaderTextAlignment = TextAlignment.Right;
        col.TextAlignment = TextAlignment.Right;
      }
    }


    public static void ReplaceColumn<T>(this GridViewAutoGeneratingColumnEventArgs args) where T : GridViewBoundColumnBase, new() {
      var col = CreateColumn<T>();
      col.Header = args.Column.Header;
      if (args.Column is GridViewDataColumn)
        col.DataMemberBinding = ((GridViewDataColumn)args.Column).DataMemberBinding;
      args.Column = col;
    }

    public static T CreateColumn<T>(Action<System.Windows.Data.Binding> bindingCallback = null) where T : GridViewBoundColumnBase, new() {
      T col = new T();
      col.IsSortable = true;
      col.IsFilterable = true;
      col.IsResizable = true;

      if (col is GridViewImageColumn) {
        GridViewImageColumn imgCol = col as GridViewImageColumn;
        imgCol.ImageWidth = 16;
        imgCol.ImageHeight = 16;
        imgCol.ImageStretch = System.Windows.Media.Stretch.Uniform;
        imgCol.ShowFieldFilters = false;
        col.TextAlignment = TextAlignment.Center;
        col.HeaderTextAlignment = TextAlignment.Center;

        col.IsResizable = false;
      } else if (col is GridViewCheckBox) {
        GridViewCheckBox chkCol = col as GridViewCheckBox;
        chkCol.IsThreeState = true;
      }

      return col;
    }


  }

}