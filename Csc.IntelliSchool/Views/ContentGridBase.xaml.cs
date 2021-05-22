using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
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

namespace Csc.IntelliSchool.Views {
  /// <summary>
  /// Interaction logic for ContentGridBase.xaml
  /// </summary>
  public partial class ContentGridBase : UserControl {


    public string Title {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(ContentGridBase), new PropertyMetadata(string.Empty));



    public Visibility ButtonVisibility {
      get { return (Visibility)GetValue(ButtonVisibilityProperty); }
      set { SetValue(ButtonVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ButtonVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ButtonVisibilityProperty =
        DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(ContentGridBase), new PropertyMetadata(Visibility.Visible));




    public object ListItems {
      get { return (object)GetValue(ListItemsProperty); }
      set { SetValue(ListItemsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ListItems.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ListItemsProperty =
        DependencyProperty.Register("ListItems", typeof(object), typeof(ContentGridBase), new PropertyMetadata(null));



    public ItemCollection Items { get { return this.ListBox.Items; } }



    public ContentGridBase() {
      InitializeComponent();
    }
  }
}
