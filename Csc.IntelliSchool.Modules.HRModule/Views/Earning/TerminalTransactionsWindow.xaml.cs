using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class TerminalTransactionsWindow : Csc.Wpf.WindowBase {
    private string _terminalIP;
    private int _userId;
    private DateTime _selectedMonth;
    private EmployeeTerminalTransaction[] _items;

    public string TerminalIP { get { return _terminalIP; } set { _terminalIP = value; OnPropertyChanged(() => TerminalIP); } }
    public int UserID { get { return _userId; } set { _userId = value; OnPropertyChanged(() => UserID); } }
    public DateTime Month { get { return _selectedMonth; } set { _selectedMonth = value; OnPropertyChanged(() => Month); } }
    public EmployeeTerminalTransaction[] Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }



    public TerminalTransactionsWindow() {
      InitializeComponent();
    }

    public TerminalTransactionsWindow( string terminalIP, int userId, DateTime month) : this() {
      TerminalIP = terminalIP;
      UserID = userId;
      Month = month;
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.SetBusy();
      EmployeesDataManager.GetTerminalTransactions(TerminalIP, UserID, Month, OnLoaded);
    }

    private void OnLoaded(EmployeeTerminalTransaction[] result, Exception error) {
      if (error == null) {
        Items = result.OrderBy(s => s.DateTime).ToArray();
        //this.ItemsGridView.SortBy("Date", "Time");
      } else
        Popup.AlertError(error);
      this.ClearBusy();
    }

    private void OKButton_Click(object sender, RoutedEventArgs e) { this.Close(OperationResult.None); }


  }

}