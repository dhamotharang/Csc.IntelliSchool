using Csc.Wpf.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;

namespace Csc.Wpf.Controls {
  public partial class GridColumnFilterPanel : UserControlBase {
    #region Properties
    public static readonly DependencyProperty GridViewProperty =
      DependencyProperty.Register("GridView", typeof(RadGridView), typeof(GridColumnFilterPanel), null);
      //new PropertyMetadata(null, (sender, e) => {
      //  GridColumnFilterPanel c = sender as GridColumnFilterPanel;
      //  if (c != null) {
      //    c.Rebind();
      //  }
      //}));

    public RadGridView GridView {
      get { return (RadGridView)GetValue(GridViewProperty); }
      set {
        SetValue(GridViewProperty, value);
        this.Rebind();
      }
    }

    public bool DataBound { get; set; }

    public List<ToggleButton> Buttons { get; set; }
    private string[] FilteredColumns { get; set; }
    public bool CanFilterGroups { get { return GridView != null && GridView.ColumnGroups.Where(s => s.HasHeader()).Count() > 0; } }
    public bool CanFilterFooter {
      get {
        if (GridView == null)
          return false;

        foreach (var col in GridView.Columns) {
          if (col.AggregateFunctions.Count() > 0) {
            return true;
          }
        }

        return false;
      }
    }
    #endregion


    #region Initializing
    public GridColumnFilterPanel() {
      InitializeComponent();
    }

    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) {
      if (this.DataBound == false)
        Rebind();
    }
    #endregion

    public void Rebind() {
      this.DataBound = false;

      OnPropertyChanged(() => CanFilterGroups);
      OnPropertyChanged(() => CanFilterFooter);
      FilterFooterButton.Rebind(RadToggleButton.IsCheckedProperty);

      Buttons = new List<ToggleButton>();
      this.FilterGroupsPanel.Children.Clear();
      this.FilterColumnsListBox.ItemsSource = null;

      if (GridView == null) {
        this.DataBound = true;
        return;
      }

      RebindGroups();
      RebindColumns();
      this.DataBound = true;
    }

    private void RebindGroups() {
      if (CanFilterGroups == false)
        return;

      foreach (var grp in GridView.ColumnGroups) {
        if (grp.HasHeader() == false)
          continue;

        RadToggleButton btn = new RadToggleButton();
        btn.Content = grp.Header;
        btn.Tag = grp.Name;
        if (Buttons.Count() == 0) { // disable first column
          btn.IsChecked = true;
          btn.IsEnabled = false;
        }
        btn.Click += FilterToggleButton_Click;
        btn.Style = this.Resources["ToggleButtonStyle"] as Style;

        Buttons.Add(btn);
      }

      PreFilterColumns();

      foreach (var btn in Buttons)
        this.FilterGroupsPanel.Children.Add(btn);

      FilterColumnGroups(FilteredColumns);
    }

    private void RebindColumns() {
      if (CanFilterGroups)
        return;

      this.FilterColumnsListBox.ItemsSource = GridView.Columns.Cast<Telerik.Windows.Controls.GridViewColumn>().Where(s => s.HasHeader()).ToArray();

    }

    private void PreFilterColumns() {
      List<string> groups = new List<string>();
      foreach (var col in GridView.Columns) {
        if (string.IsNullOrEmpty(col.ColumnGroupName) == false && string.IsNullOrEmpty(col.Name) == false) {
          if (groups.Contains(col.ColumnGroupName.ToLower()))
            continue;

          groups.Add(col.ColumnGroupName.ToLower());
          ToggleButton btn = Buttons.SingleOrDefault(s => s.Tag.ToString() == col.ColumnGroupName);
          if (btn == null)
            continue;
          btn.IsChecked = col.IsVisible;
        }
      }
    }


    #region Filter
    public void FilterColumnGroupsInclude(params string[] colGroups) {
      if (FilteredColumns == null || FilteredColumns.Intersect(colGroups).Count() != colGroups.Count())
        FilterColumnGroups((FilteredColumns != null ? FilteredColumns : new string[] { }).Concat(colGroups).ToArray());
    }
    public void FilterColumnGroupsExclude(params string[] colGroups) {
      if (FilteredColumns != null && FilteredColumns.Intersect(colGroups).Count() != 0)
        FilterColumnGroups((FilteredColumns != null ? FilteredColumns : new string[] { }).Except(colGroups).ToArray());
    }
    public void FilterColumnGroups(params string[] colGroups) {
      FilteredColumns = colGroups;
      if (Buttons == null || colGroups == null)
        return;

      foreach (var btn in Buttons) {
        btn.IsChecked = colGroups.Contains(btn.Tag.ToString());
        FilterColumns(btn);
      }
    }

    public void FilterColumnGroups(RadGridView grid, params string[] colGroups) {
      FilteredColumns = colGroups;
      if (null != grid && this.GridView != grid) // avoid duplication
        this.GridView = grid;
      else
        FilterColumnGroups(colGroups);
    }

    private void FilterColumns(ToggleButton btn) {
      this.GridView.FilterDescriptors.SuspendNotifications();
      try {
        var tag = btn.Tag.ToString();
        foreach (var col in GridView.Columns) {
          if (col.ColumnGroupName == tag) {
            col.IsVisible = btn.IsChecked == true;
            if (col.IsVisible == false)
              col.ColumnFilterDescriptor.Clear();
          }
        }
      } finally {
        this.GridView.FilterDescriptors.ResumeNotifications();
      }
    }

    private void FilterToggleButton_Click(object sender, RoutedEventArgs e) {
      var btn = (RadToggleButton)sender;
      this.GridView.FilterColumns(btn.Tag.ToString(), btn.IsChecked == true);
    }
    #endregion


    #region expand / collapse
    //private void ExpandButton_Click(object sender, RoutedEventArgs e) {
    //  if (null == this.GridView)
    //    return;

    //  this.GridView.ExpandAllGroups();

    //  foreach (var row in this.GridView.Rows()) {
    //    row.IsExpanded = row.IsExpandable;
    //  }

    //  //this.GridView.ExpandAllHierarchyItems();
    //}


    //private void CollapseButton_Click(object sender, RoutedEventArgs e) {
    //  if (null == this.GridView)
    //    return;

    //  //foreach (var row in this.GridView.Rows()) {
    //  //  row.IsExpanded = false;
    //  //}

    //  this.GridView.CollapseAllHierarchyItems();
    //  this.GridView.CollapseAllGroups();
    //}
    #endregion


    #region Clear Filter
    private void ClearFiltersButton_Click(object sender, RoutedEventArgs e) {
      if (null == this.GridView)
        return;

      this.GridView.FilterDescriptors.SuspendNotifications();
      try {
        foreach (var col in this.GridView.Columns)
          col.ColumnFilterDescriptor.Clear();
      } finally {
        this.GridView.FilterDescriptors.ResumeNotifications();

      }
      //this.GridView.FilterDescriptors.Clear();

    }
    #endregion

    #region Export
    private void ExportButton_Click(object sender, RoutedEventArgs e) {
      if (null == this.GridView)
        return;

      this.GridView.ExportAsync();
    }
    #endregion

    private void AdvancedFiltersButton_Click(object sender, RoutedEventArgs e) {
      if (null == this.GridView)
        return;

      AdvancedFiltersWindow wnd = new AdvancedFiltersWindow(this.GridView);
      this.GridView.DisplayModal(wnd);
    }
  }
}
