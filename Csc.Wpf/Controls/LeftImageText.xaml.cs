using System.Windows;
using System.Windows.Media;

namespace Csc.Wpf.Controls {
  public partial class LeftImageText : UserControlBase {
    public int ImageWidth {
      get { return (int)GetValue(ImageWidthProperty); }
      set { SetValue(ImageWidthProperty, value); }
    }
    public int ImageHeight {
      get { return (int)GetValue(ImageHeightProperty); }
      set { SetValue(ImageHeightProperty, value); }
    }
    public string Text {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }
    public ImageSource Image {
      get { return (ImageSource)GetValue(ImageProperty); }
      set { SetValue(ImageProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(LeftImageText), new PropertyMetadata(""));
    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(ImageSource), typeof(LeftImageText), new PropertyMetadata(null));
    public static readonly DependencyProperty ImageWidthProperty =
       DependencyProperty.Register("ImageWidth", typeof(int), typeof(LeftImageText), new PropertyMetadata(18));
    public static readonly DependencyProperty ImageHeightProperty =
        DependencyProperty.Register("ImageHeight", typeof(int), typeof(LeftImageText), new PropertyMetadata(18));



    public LeftImageText() {
      InitializeComponent();
    }
  }
}
