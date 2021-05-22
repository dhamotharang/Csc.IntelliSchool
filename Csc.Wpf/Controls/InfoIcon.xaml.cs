using System.Windows;
using System.Windows.Media;

namespace Csc.Wpf.Controls {
  public partial class InfoIcon : UserControlBase {
    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(InfoIcon), new PropertyMetadata(""));



    public InfoIcon() {
      InitializeComponent();
    }
  }
}
