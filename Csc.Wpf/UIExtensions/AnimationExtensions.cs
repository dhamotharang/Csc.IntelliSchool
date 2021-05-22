using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

namespace Csc.Wpf {
  public static partial class AnimationExtensions {
    public static void ShowAnimated(this UIElement elem, Action completed = null) {
      elem.Visibility = Visibility.Visible;

      DoubleAnimation animation = new DoubleAnimation();
      animation.To = 1;
      animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

      animation.Completed += (o, e) => {
        if (null != completed)
          completed();
      };

      elem.BeginAnimation(UIElement.OpacityProperty, animation);
    }

    public static void HideAnimated(this UIElement elem, Action completed = null) {
      DoubleAnimation animation = new DoubleAnimation();
      animation.To = 0;
      animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

      animation.Completed += (o, e) => {
        elem.Visibility = Visibility.Collapsed;
        if (null != completed)
          completed();
      };

      elem.BeginAnimation(UIElement.OpacityProperty, animation);
    }

  }
}