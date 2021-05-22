using System;
using System.Linq;
using Csc.Wpf;
using System.Windows;
using Telerik.Windows.Controls;
using System.Collections.Generic;
using System.Linq.Expressions;
using Csc.Components.Common;

namespace Csc.Wpf.Views {
  public partial class ItemsListWindow : Csc.Wpf.WindowBase   {
    #region Fields
    private Action<AsyncState<IEnumerable<object>>> _itemsCallback;
    //private Expression<Func<object>> _displayMember;
    #endregion

    #region Properties
    public Action<AsyncState<IEnumerable<object>>> ItemsCallback { get { return _itemsCallback; } set { if (_itemsCallback != value) { _itemsCallback = value; OnPropertyChanged(() => ItemsCallback); } } }

    private string _displayMemberPath;
    public string DisplayMemberPath { get { return _displayMemberPath; } set { if (_displayMemberPath != value) { _displayMemberPath = value; this.ItemsList.DisplayMemberPath = value; OnPropertyChanged(() => DisplayMemberPath); } } }

    //public Expression<Func<object>> DisplayMember
    //{
    //  get { return _displayMember; }
    //  set
    //  {
    //    if (_displayMember != value) {
    //      _displayMember = value;
    //      this.ItemsList.DisplayMemberPath = ((MemberExpression)value .Body).Member.Name;
    //      OnPropertyChanged(() => DisplayMember);
    //    }
    //  }
    //}
    #endregion


    public ItemsListWindow() {
      InitializeComponent();
    }


    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.SetBusy();
      ItemsCallback(OnItemsLoaded);
    }

    private void OnItemsLoaded(IEnumerable<object> result, Exception error) {
      if (result != null) {
        this.ItemsList.ItemsSource = result;
      } else
        this.AlertError(error);
      this.ClearBusy();
    }


    public IEnumerable<T> SelectedItemsAs<T>() where T : class {
      return this.ItemsList.SelectedItemsAs<T>();
    }


    private void CancelButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }

    private void SelectButton_Click(object sender, RoutedEventArgs e) {
      if (this.ItemsList.SelectedItems.Count () == 0) {
        this.AlertError("No selected items.");
        return;
      }

      this.Close(true);
    }
  }
}
