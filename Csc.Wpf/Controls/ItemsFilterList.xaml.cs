using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.Wpf.Controls {
  public partial class ItemsFilterList : UserControlBase {
    #region Dependency Properties
    public static readonly DependencyProperty GridViewProperty =
      DependencyProperty.Register("GridView", typeof(RadGridView), typeof(UserControlBase),
      new PropertyMetadata(null, (sender, e) => {
        ItemsFilterList c = sender as ItemsFilterList;
        c.Rebind();
      }));
    #endregion


    #region Fields
    private object[] _allItems;
    //private object[] _filteredItems;
    private object[] _selectedItems;
    private object[] _CurrentItems;
    #endregion

    #region Properties
    public object[] AllItems { get { return _allItems; } set { _allItems = value; OnPropertyChanged(() => AllItems); } }
    //public object[] FilteredItems { get { return _filteredItems; } set { _filteredItems = value; OnPropertyChanged(() => FilteredItems); } }
    public object[] SelectedItems { get { return _selectedItems; } set { _selectedItems = value; OnPropertyChanged(() => SelectedItems); } }
    public object[] CurrentItems { get { return _CurrentItems; } set { _CurrentItems = value; OnPropertyChanged(() => CurrentItems); } }
    public RadGridView GridView { get { return (RadGridView)GetValue(GridViewProperty); } set { SetValue(GridViewProperty, value); } }

    public bool IsValid { get { return this.AllItemsButton.IsChecked == true || this.SelectedItemsButton.IsChecked == true || this.CurrentItemsButton.IsChecked == true; } }
    public object[] Items {
      get {
        if (IsValid == false)
          return null;

        if (this.AllItemsButton.IsChecked == true)
          return AllItems;
        //else if (this.FilteredItemsButton.IsChecked == true)
        //  return FilteredItems;
        else if (this.SelectedItemsButton.IsChecked == true)
          return SelectedItems;
        else if (this.CurrentItemsButton.IsChecked == true)
          return CurrentItems;
        else
          return null;
      }
    }
    #endregion

    public ItemsFilterList() {
      InitializeComponent();
    }


    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) {
    }

    public void Rebind() {
        AllItems = GridView != null ? (GridView.ItemsSource as IEnumerable<object>).ToArray() : null;
      //FilteredItems = GridView != null ? (GridView.ItemsSource as IEnumerable<object>).ToArray() : null;
      SelectedItems = GridView != null ? GridView.SelectedItems.ToArray() : null;
      CurrentItems = GridView != null ? GridView.Items.Cast<object>().ToArray() : null;
    }


  }
}
