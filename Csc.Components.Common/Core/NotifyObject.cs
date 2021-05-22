using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Csc.Components.Common {
  public class NotifyObject : INotifyPropertyChanged {
    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    protected bool NotificationSuspended { get; private set; }

    #endregion

    public NotifyObject() { }

    #region Property Change Notify
    protected void SuspendNotification() { NotificationSuspended = true; }
    protected void ResumeNotification() { NotificationSuspended = false; }
    protected void OnPropertyChanged<T>(Expression<Func<T>> expr, bool force = false) { this.OnPropertyChanged(((MemberExpression)expr.Body).Member.Name, force); }
    protected void OnPropertyChanged(string propName, bool force = false) {
      if (NotificationSuspended && force == false)
        return;

      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
    #endregion
  }
}