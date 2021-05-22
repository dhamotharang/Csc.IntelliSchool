using System;
using System.Drawing;
using System.IO;

namespace Csc.Components.Imaging {
  public static class ImageHandler {

    public static Image ScaleImage(Stream imageStream, int maxWidth, int maxHeight) {
      return ScaleImage(Image.FromStream(imageStream), maxWidth, maxHeight);

    }
    public static Image ScaleImage(Image image, int maxWidth, int maxHeight) {
      if (image.Width <= maxWidth && image.Height <= maxHeight) // resize if applicable only
        return image;

      var ratioX = (double)maxWidth / image.Width;
      var ratioY = (double)maxHeight / image.Height;
      var ratio = Math.Min(ratioX, ratioY);

      int newWidth = (int)(image.Width * ratio);
      int newHeight = (int)(image.Height * ratio);

      var newImage = new Bitmap(newWidth, newHeight);

      using (var graphics = Graphics.FromImage(newImage))
        graphics.DrawImage(image, 0, 0, newWidth, newHeight);

      return newImage;
    }
  }
}
