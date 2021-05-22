using System;
using System.Linq;
using Csc.Wpf;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.DataFilter;
using System.Collections.Generic;
using System.Diagnostics;

namespace Csc.Wpf.Views {
  public class DataFilterProperty {
    public string Path { get; set; }
    public string Header { get; set; }
    public Type Type { get; set; }
    public int Index { get; set; }
    public ItemPropertyDefinition Definition { get; set; }

    public DataFilterProperty() { }
    public DataFilterProperty(string path, string header) {
      Path = path;
      Header = header;
    }
  }

  public partial class AdvancedFiltersWindow : Csc.Wpf.WindowBase {
    private RadGridView _gridView;

    public List<DataFilterProperty> FilterProperties { get; set; }
    public RadGridView GridView {
      get { return _gridView; }
      set {
        if (_gridView != value) {
          _gridView = value;
          OnPropertyChanged(() => GridView);
          LoadFilterColumns();
        }
      }
    }


    public AdvancedFiltersWindow() { InitializeComponent();       FilterProperties = new List<DataFilterProperty>();}
    public AdvancedFiltersWindow(RadGridView grid) : this() { this.GridView = grid; }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      this.DataFilter.FilterDescriptors.Clear();
      this.Close(OperationResult.None);
    }
    private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e) {
      //if (Items == null) {
      //  this.AlertError("No selected items.");
      //  return;
      //}

      //if (Items.Count() == 0) {
      //  this.Close(OperationResult.None);
      //  return;
      //}

      this.Close(true);
    }

  
    private void LoadFilterColumns() {
      FilterProperties.Clear();
      
      if (this.GridView == null)
        return;

      foreach (var col in this.GridView.Columns) {
        GridViewBoundColumnBase dataCol = col as GridViewBoundColumnBase;

        if (dataCol == null || dataCol.Header == null || dataCol.DataMemberBinding == null || dataCol.DataMemberBinding.Path == null)
          continue;

        var prop = new DataFilterProperty();

        prop.Header = string.Empty;
        if (string.IsNullOrWhiteSpace(dataCol.ColumnGroupName) == false)
          prop.Header += dataCol.ColumnGroupName + " > ";
        prop.Header += dataCol.Header.ToString();

        prop.Path = dataCol.DataMemberBinding.Path.Path;

        prop.Type = dataCol.DataType;

        prop.Definition = new ItemPropertyDefinition(prop.Path, prop.Type, prop.Header);

        FilterProperties.Add(prop);

        this.DataFilter.ItemPropertyDefinitions.Add(prop.Definition);
      }
    }
  }
}
