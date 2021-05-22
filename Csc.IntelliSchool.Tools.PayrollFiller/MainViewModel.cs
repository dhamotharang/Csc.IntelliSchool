using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace Csc.IntelliSchool.Tools.PayrollFiller {
  public class MainViewModel : INotifyPropertyChanged {
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged<T>(Expression<Func<T>> expr) {
      OnPropertyChanged(((MemberExpression)expr.Body).Member.Name);
    }
    public void OnPropertyChanged(string name) {
      if (null != PropertyChanged)
        PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
    #endregion

    #region Fields
    private string _filename;
    private string[] _worksheets;
    private DateTime? _selectedMonth;
    private bool _isBusy;
    private int _selectedSheetIndex = -1;
    #endregion

    #region Properties
    public string Filename {
      get { return _filename; }
      set {
        if (value != _filename) {
          _filename = value;
          OnPropertyChanged(() => Filename);
        }
      }
    }
    public string[] Worksheets {
      get { return _worksheets; }
      set {
        if (value != _worksheets) {
          _worksheets = value;
          OnPropertyChanged(() => Worksheets);
        }
      }
    }
    public DateTime? SelectedMonth {
      get { return _selectedMonth; }
      set {
        if (value != _selectedMonth) {
          if (value == null)
            _selectedMonth = value;
          else
            _selectedMonth = new DateTime(value.Value.Year, value.Value.Month, 1);
          OnPropertyChanged(() => SelectedMonth);
        }
      }
    }
    public bool IsBusy {
      get {
        return _isBusy;
      }
      set {
        if (_isBusy != value) {
          _isBusy = value;
          OnPropertyChanged(() => IsBusy);
        }
      }
    }
    public int SelectedSheetIndex {
      get {
        return _selectedSheetIndex;
      }
      set {
        if (_selectedSheetIndex != value) {
          _selectedSheetIndex = value;
          OnPropertyChanged(() => SelectedSheetIndex);
        }
      }
    }
    #endregion


    public bool SelectFile(Window wnd = null) {
      var filename = Functions.SelectFile(wnd, dlg => {
        dlg.Filter = "Excel Files (*.xlsx)|*.xlsx";
        dlg.Title = "Select Excel File";
      });

      if (filename == null)
        return false;

      this.Filename = filename;
      return true;
    }


    public void LoadWorksheetsAsync() {
      SetBusy();
      Task.Factory.StartNew(LoadWorksheets).ContinueWith(a => {
        Application.Current.Dispatcher.Invoke(new Action(ClearBusy));
      });
    }
    public void LoadWorksheets() {
      if (Filename == null)
        throw new ArgumentNullException(nameof(Filename));

      SelectedSheetIndex = -1;

      Worksheets = Functions.GetWorksheetNames(Filename);
      if (Worksheets.Length == 1)
        SelectedSheetIndex = 0;
    }

    public void SetBusy() { IsBusy = true; }
    public void ClearBusy() { IsBusy = false; }


    public void FillDataAsync() {
      SetBusy();
      Task.Factory.StartNew(FillData).ContinueWith(a => {
        Application.Current.Dispatcher.Invoke(new Action(ClearBusy));
      });
    }
    public void FillData() {
      DataFillingTask task = new DataFillingTask(Filename, SelectedSheetIndex + 1, SelectedMonth.Value);
      task.Run();
    }

  }
}
