using Csc.Components.Common;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Csc.Wpf {
  public class PageBase : Page, INotifyPropertyChanged, ILock {
    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Lock
    private object _lockObject;
    public object LockObject {
      get {
        if (_lockObject == null)
          lock (this)
            if (_lockObject == null)
              _lockObject = new object();
        return _lockObject;
      }
    }
    #endregion

    public virtual bool DataInitialized { get { return false; } }

    public PageBase() {
      this.FocusFirstChild();
    }

    private void FocusFirstChild() { 
    
    }


    #region Property Change Notify
    protected void OnPropertyChanged<T>(Expression<Func<T>> expr) { this.OnPropertyChanged(((MemberExpression)expr.Body).Member.Name); }
    protected void OnPropertyChanged(string propName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName)); }
    #endregion

  }
}