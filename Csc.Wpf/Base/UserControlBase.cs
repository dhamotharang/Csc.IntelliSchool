using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public class UserControlBase : UserControl, INotifyPropertyChanged {
    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Fields
    private object _lockObj = new object();
    private RadTabItem _parentTab;
    private bool _dataLoaded;
    public virtual bool HasUpdates { get { return false; } }
    #endregion

    #region Properties
    public RadTabItem ParentTab {
      get {
        if (_parentTab == null)
          _parentTab = this.ParentOfType<RadTabItem>();
        return _parentTab;
      }
    }
    public bool ParentTabSelected {
      get {
        return ParentTab != null && ParentTab.IsSelected;
      }
    }
    // TODO: find better way
    public virtual bool DataInitialized { get { return false; } }
    public bool DataLoaded { get { return _dataLoaded; } set { _dataLoaded = value; OnPropertyChanged(() => IsInitialized); } }
    #endregion

    public UserControlBase() { }

    #region Property Change Notify
    protected void OnPropertyChanged<T>(Expression<Func<T>> expr) { this.OnPropertyChanged(((MemberExpression)expr.Body).Member.Name); }
    protected void OnPropertyChanged(string propName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName)); }
    #endregion
  }
}