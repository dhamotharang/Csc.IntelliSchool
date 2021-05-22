using Csc.Components.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Csc.Wpf {
  public class ViewRoot : ContentControl, IBusy, INotifyPropertyChanged, ILock  {
    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler BusyChanged;
    public event EventHandler CurrentPageChanged;
    #endregion

    #region Fields
    private IView[] _views;
    private IView[] _allViews;
    private IView _currentView;
    private int _busyCounter = 0;
    private bool _isBusy = false;
    private object _lockObject = null;
    private IView _homeView;
    private Page _currentPage;
    #endregion

    #region Properties
    public IView[] Views { get { return _views; } protected set { _views = value; OnPropertyChanged(() => Views); } }
    public IView[] AllViews { get { return _allViews; } set { _allViews = value; OnPropertyChanged(() => AllViews); } }
    public IView CurrentView { get { return _currentView; } set { _currentView = value; OnPropertyChanged(() => CurrentView); } }
    public Page CurrentPage { get { return _currentPage; } protected set { _currentPage = value; OnPropertyChanged(() => CurrentPage); } }
    public IView HomeView { get { return _homeView; } protected set { _homeView = value; OnPropertyChanged(() => HomeView); } }

    public bool IsBusy {
      get { return _isBusy; }
      protected set {
        bool changed = _isBusy != value;
        _isBusy = value;
        if (changed)
          OnBusyChanged();
      }
    }
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

    static ViewRoot() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewRoot), new FrameworkPropertyMetadata(typeof(ViewRoot)));
    }

    #region Initialization
    public void LoadViews(IEnumerable<IView> views) {
      lock (LockObject) {
        Views = views.ToArray();
        AllViews = views.SelectHierarchy<IView>(s => s.ChildViews);
        HomeView = null;
        SetCurrentView(null, null);

        if (Views != null)
          HomeView = AllViews.Where(s => s.IsHome).FirstOrDefault();
        if (HomeView == null)
          HomeView = AllViews.Where(s => string.IsNullOrEmpty(s.Path) == false).FirstOrDefault();
      }
    }
    #endregion


    #region Navigation
    public void NavigateNull() { NavigateTo(null); }
    public void NavigateHome() { NavigateTo(HomeView); }
    public void NavigateTo(IView view) {
      bool hasChanges = false;

      lock (LockObject) {
        Page page = null;

        if (null != view) {
          if (AllViews.Contains(view) == false)
            throw new ArgumentException();

          page = CreatePage(view);
        }

        hasChanges = SetCurrentView(view, page);
      }

      if (hasChanges)
        OnCurrentPageChanged();
    }

    private static Page CreatePage(IView view) {
      // Getting page type
      string fullPath = view.Path;
      if (view.Assembly != null && view.Assembly.Length > 0)
        fullPath += ", " + view.Assembly;

      Type type = Type.GetType(fullPath, true, true);

      // creating page
      Page page = (Page)Activator.CreateInstance(type);

      // setting parameters
      var paramList = GetViewParams(view);
      foreach (var param in paramList)
        SetParamValue(page, type, param.Param, param.Value);
      return page;
    }

    private static ViewParam[] GetViewParams(IView view) {
      List<ViewParam> paramList = new List<ViewParam>();

      Action<string> fillParams = (paramStr) => {
        if (paramStr != null) {
          var parameters = paramStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
          foreach (var param in parameters) {
            var splits = param.Split('=');
            if (splits.Length != 2)
              continue;

            var propName = splits[0].Trim();
            var value = splits[1].Trim();

            if (propName.Length == 0) continue;

            paramList.Add(new ViewParam(propName, value));
          }
        }
      };


      fillParams(view.Params);
      fillParams(view.UserParams);

      return paramList.ToArray();
    }

    private static void SetParamValue(Page page, Type type, string propName, string value) {
      var prop = type.GetProperty(propName);

      if (prop == null)
        return;

      Type propType;

      if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        propType = prop.PropertyType.GetGenericArguments()[0];
      else
        propType = prop.PropertyType;


      prop.SetValue(page, Convert.ChangeType(value, propType));
    }

    private bool SetCurrentView(IView view, Page page) {
      CurrentView = view;

      if (CurrentPage != page) {
        CurrentPage = page;
        return true;
      }

      return false;
    }

    protected void OnCurrentPageChanged() {
      this.BeginInvoke(() => {
        if (CurrentPageChanged != null)
          CurrentPageChanged(this, EventArgs.Empty);
      });
    }
    #endregion

    #region Busy
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
        });
      }
    }
    protected void OnBusyChanged() {
      this.BeginInvoke(() => {
        OnPropertyChanged(() => IsBusy);
        if (BusyChanged != null)
          BusyChanged(this, EventArgs.Empty);
      });
    }
    #endregion


    #region Property Change Notify
    protected void OnPropertyChanged<T>(Expression<Func<T>> expr) { this.OnPropertyChanged(((MemberExpression)expr.Body).Member.Name); }
    protected void OnPropertyChanged(string propName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName)); }
    #endregion
  }
}
