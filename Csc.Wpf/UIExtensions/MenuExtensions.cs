using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Csc.Wpf {
  public static partial class MenuExtensions {
    public static RadMenuItem FindMenuItem(this RadContextMenu menu, string name) {
      return menu.Items().SingleOrDefault(s => s.Name == name);
    }

    public static GridViewRow GetClickedRow(this RadContextMenu menu) {
      return menu.GetClickedElement<GridViewRow>();
    }

    public static GridViewRow GetClickedRow(this RadMenuItem item) {
      return (item.Menu as RadContextMenu).GetClickedRow();
    }



    public static void OpenContextMenu(this FrameworkElement elem) {
      var menu = Telerik.Windows.Controls.RadContextMenu.GetContextMenu(elem);
      menu.PlacementTarget = elem;
      menu.IsOpen = true;
    }
  }
}