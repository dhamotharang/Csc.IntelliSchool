using System;
using System.Linq; 
using Csc.Wpf; 
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.Wpf.Views {
  public partial class ItemsFilterWindow : Csc.Wpf.WindowBase {
    #region Properties
    public RadGridView GridView { get { return this.FilterList.GridView; } set { this.FilterList.GridView = value; OnPropertyChanged(() => GridView); } }
    public object[] Items { get { return this.FilterList.Items != null ? this.FilterList.Items.Select(s => s).ToArray() : null; } }
    #endregion

    public ItemsFilterWindow() {
      InitializeComponent();
    }
    public ItemsFilterWindow(RadGridView grid,  string header = null)  : this(){
      if (null != header)
        this.Header = header;
      this.GridView = grid;
      this.FilterList.Rebind();
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {

    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }

    private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e) {
      if (Items == null) {
        this.AlertError("No selected items.");
        return;
      }

      if (Items.Count() == 0) {
        this.Close(OperationResult.None);
        return;
      }

      this.Close(true);
    }


  }
}
