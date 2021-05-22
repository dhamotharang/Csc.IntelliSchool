using System;
using System.Windows.Input;

namespace Csc.Wpf {
  public class RelayCommand : ICommand {
    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    private Action<object> _execute;
    private Func<object, bool> _canExecute;


    public RelayCommand(Action execute, Func<bool> canExecute = null) : this((obj) => execute(), (obj) => canExecute()) {
    }

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) {
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public bool CanExecute(object parameter) {
      return this._canExecute == null || this._canExecute(parameter);
    }

    public void Execute(object parameter) {
      this._execute(parameter);
    }
  }
}
