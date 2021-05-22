using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Csc.IntelliSchool.Tools.PayrollFiller {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainViewModel ViewModel { get { return this.DataContext as MainViewModel; } }


    public MainWindow() {
      InitializeComponent();
      SetMonthPickerFormat();
      this.DataContext = new MainViewModel() { SelectedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month , 1) };

  
    }

    private void SetMonthPickerFormat() {
      this.MonthDatePicker.Culture = new System.Globalization.CultureInfo("en-GB");
      this.MonthDatePicker.Culture.DateTimeFormat.LongDatePattern = "MMMM, yyyy";
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e) {
      if (true == ViewModel.SelectFile(Application.Current.MainWindow)) {
        ViewModel.LoadWorksheetsAsync();
      }
    }

    private void FillButton_Click(object sender, RoutedEventArgs e) {
      if (ViewModel.Filename == null) {
        MsgBox("Please select file.");
        return;
      }
      if (ViewModel.SelectedSheetIndex == -1) {
        MsgBox("Please select sheet");
        return;
      }
      if (ViewModel.SelectedMonth == null) {
        MsgBox("Please select month");
        return;
      }


      ViewModel.FillDataAsync();
    }

    private static void MsgBox(string msg, MessageBoxImage img = MessageBoxImage.None) {
      MessageBox.Show(Application.Current.MainWindow, msg, Application.Current.MainWindow.Title, MessageBoxButton.OK, img);
    }
  }


}
