using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Csc.Components.Imaging {
  public static class ImageExtensions {
    public static System.Windows.Media.ImageSource LoadImageSource(string uri, UriKind uriKind = UriKind.RelativeOrAbsolute) {
      return LoadImageSource(new Uri(uri, uriKind));
    }
    public static System.Windows.Media.ImageSource LoadImageSource(Uri uri) {
      BitmapImage image = null;

      try {
        image = new BitmapImage(uri);
      } catch (IOException) {

      }

      return image;
    }

  }
}
