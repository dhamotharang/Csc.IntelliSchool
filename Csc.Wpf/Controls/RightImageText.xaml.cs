using System.Windows;
using System.Windows.Media;

namespace Csc.Wpf.Controls {
  public partial class RightImageText : UserControlBase {
    public int ImageWidth {
      get { return (int)GetValue(ImageWidthProperty); }
      set { SetValue(ImageWidthProperty, value); }
    }
    public int ImageHeight {
      get { return (int)GetValue(ImageHeightProperty); }
      set { SetValue(ImageHeightProperty, value); }
    }
    public object Text {
      get { return (object)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }
    
    public ImageSource Image {
      get { return (ImageSource)GetValue(ImageProperty); }
      set { SetValue(ImageProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(object), typeof(RightImageText), new PropertyMetadata(null));
    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(ImageSource), typeof(RightImageText), new PropertyMetadata(null));
    public static readonly DependencyProperty ImageWidthProperty =
       DependencyProperty.Register("ImageWidth", typeof(int), typeof(RightImageText), new PropertyMetadata(18));
    public static readonly DependencyProperty ImageHeightProperty =
        DependencyProperty.Register("ImageHeight", typeof(int), typeof(RightImageText), new PropertyMetadata(18));


    public RightImageText() {
     InitializeComponent();
    }
  }
}
