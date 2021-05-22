using Csc.Components.Common;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public enum OperationResult { None = 0, Add, Update, Delete}

  public  class WindowBase : RadWindow, INotifyPropertyChanged, IBusy, ILock {
    #region Events
    public event EventHandler BusyChanged;
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Fields
    private bool _dataLoaded;
    private OperationResult _operationResult;
    private int _busyCounter = 0;
    private bool _isBusy = false;
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


    #region Properties
    public virtual bool HasUpdates { get { return false; } }
    public  bool IsBusy {
      get { return _isBusy; }
      protected set {
        bool changed = _isBusy != value;
        _isBusy = value;
        if (changed)
          OnBusyChanged();
      }
    }

    public bool AutoRejectChanges { get; set; }
    public bool DataLoaded { get { return _dataLoaded; } set { _dataLoaded = value; OnPropertyChanged(() => IsInitialized); } }
    public OperationResult Result { get { return _operationResult; }protected set { _operationResult = value; OnPropertyChanged(() => Result); } }
    #endregion

    public WindowBase() {
      this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
      this.ResizeMode = System.Windows.ResizeMode.NoResize;
      this.HideMaximizeButton = true;
      this.HideMinimizeButton = true;
      this.AutoRejectChanges = true;
      Result = OperationResult.None;
      this.KeyUp += WindowBase_KeyUp;
    }

    void WindowBase_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
      //if (e.Key == System.Windows.Input.Key.Escape && IsBusy == false)
      //  this.Close();
    }

    #region Methods
    public void SetBusy(bool busy) {
      lock (LockObject) {
        if (busy)
          _busyCounter++;
        else
          _busyCounter--;
        if (_busyCounter < 0)
          _busyCounter = 0;

        IsBusy = _busyCounter > 0;

        this.BeginInvoke(() => {
          this.IsEnabled = busy == false;
          this.CanClose = busy == false;
        });
      }
    }
    protected void OnBusyChanged() { OnPropertyChanged(() => IsBusy); if (BusyChanged != null)BusyChanged(this, EventArgs.Empty); }
    
    public void Close( OperationResult operationResult) {
      Close(operationResult != OperationResult.None, operationResult);
    }
    public void Close(bool result) {
      this.DialogResult = result;
      Result = result ? OperationResult.Update : OperationResult.None;
      this.Close();
    }
    public void Close(bool result, OperationResult operationResult) {
      this.DialogResult = result;
      Result = operationResult;
      this.Close();
    }
    #endregion

    #region Property Change Notify
    protected void OnPropertyChanged<T>(Expression<Func<T>> expr) { this.OnPropertyChanged(((MemberExpression)expr.Body).Member.Name); }
    protected void Rebind<T>(Expression<Func<T>> expr) { OnPropertyChanged<T>(expr); }
    protected void Rebind() {
      var tmpContext = this.DataContext;
      this.DataContext = null;
      this.DataContext = tmpContext;
    }
    protected void OnPropertyChanged(string propName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName)); }
    #endregion
  }
}
