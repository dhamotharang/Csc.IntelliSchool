using System.Collections;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.Wpf.Controls {
  public partial class ItemsList : UserControlBase {
    #region Dependency Properties
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsList), new PropertyMetadata(null));
    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ItemsList), new PropertyMetadata(null));
    #endregion

    #region Properties
    public IEnumerable ItemsSource { get { return (IEnumerable)GetValue(ItemsSourceProperty); } set { SetValue(ItemsSourceProperty, value); } }
    public string DisplayMemberPath { get { return (string)GetValue(DisplayMemberPathProperty); } set { SetValue(DisplayMemberPathProperty, value); } }

    public ICollection<object> SelectedItems { get { return this.ItemsTreeView.CheckedItems();} }
    #endregion


    public ItemsList() {
      InitializeComponent();
    }

    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) {

    }

    #region Selection
    public IEnumerable<T> SelectedItemsAs<T>() where T : class{
      return this.ItemsTreeView.CheckedItemsAs<T>();
    }

    public void SelectAll() { SetAllSelection(true);}
    public void DeselectAll() { SetAllSelection(false ); }
    protected void SetAllSelection(bool selected) {
      foreach (var itm in this.ItemsTreeView.Items())
        itm.CheckState = selected ? System.Windows.Automation.ToggleState.On : System.Windows.Automation.ToggleState.Off;
    }
    #endregion

    #region Helpers
    private void ItemsTreeView_CheckChanged(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var selCount = SelectedItems.Count;
      this.SelectCheckBox.IsChecked = selCount == ItemsTreeView.Items.Count ? true : selCount == 0 ? false : new bool?();
    }

    private void SelectCheckBox_Click(object sender, RoutedEventArgs e) {
      if (this.SelectCheckBox.IsChecked == true)
        SelectAll();
      else {
        this.SelectCheckBox.IsChecked = false;
        DeselectAll();
      }
    }
    #endregion
  }
}
