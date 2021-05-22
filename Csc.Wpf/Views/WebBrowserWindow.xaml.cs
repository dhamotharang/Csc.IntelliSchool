namespace Csc.Wpf.Views {
  public partial class WebBrowserWindow : Csc.Wpf.WindowBase {
    public WebBrowserWindow(string title, string filename) {
      InitializeComponent();

      this.Header = title;
      this.WebBrowser.Navigate(filename);
    }

    private void WebBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e) {
    }



  }
}
