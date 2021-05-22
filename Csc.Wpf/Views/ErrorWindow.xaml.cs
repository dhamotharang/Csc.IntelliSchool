using System.Windows;

namespace Csc.Wpf.Views {
  public partial class ErrorWindow : WindowBase {

    public string Message { get; set; }
    public string Details { get; set; }

    public ErrorWindow() {
      InitializeComponent();
    }

    private void OKButton_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }

    private void WindowBase_Loaded(object sender, RoutedEventArgs e) {
      this.MessageTextBox.Text = Message;
      DetailsButton.Visibility = string.IsNullOrEmpty(Details) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void DetailsButton_Click(object sender, RoutedEventArgs e) {
      Popup.Alert(this, Details);
    }
  }
}
