using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using Csc.IntelliSchool.Views.Account;
using Csc.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using System;

namespace Csc.IntelliSchool {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      Csc.Wpf.Performance.ApplyBitmapRecommendations(this);

      AccountDataManager.SignedOut += AccountDataManager_SignedOut;
      this.ContentGrid.Opacity = 0;
    }



    #region Loading
    private void Window_Loaded(object sender, RoutedEventArgs e) {
      this.Title = App.Title;
      OnLogin();
    }

    #endregion

    #region NavigationTreeView
    private void NavigationTreeView_Loaded(object sender, RoutedEventArgs e) {
      RadTreeView tree = sender as RadTreeView;
      tree.ExpandAll();
    }

    private void NavigationTreeView_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      RadTreeView tree = sender as RadTreeView;
      var itm = tree.SelectedItem as View;

      if (itm == null || itm.Path == null)
        return;

      NavigateTo(itm);

      //this.NavigationExpander.IsExpanded = false;
    }
    #endregion

    //#region NavigationMenu
    //private void NavigationMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e) {
    //  var itm = ((RadMenuItem)e.OriginalSource).DataContext as View;

    //  if (itm == null || itm.Path == null)
    //    return;

    //  NavigateTo(itm);

    //}
    //#endregion

    #region NavigationOutlookBar 
    protected void LoadNavigationItems(View[] views) {
      this.NavigationOutlookBar.Items.Clear();

      var groups = views.Where(s => s.Group != null).Select(s => s.Group).Distinct().OrderBy(s=>s.Order).ThenBy(s=>s.Name).ToArray();
      foreach (var grp in groups) {
        RadOutlookBarItem item = new RadOutlookBarItem();

        item.Header  = grp.Name;
        item.ToolTip = grp.Description;
        if (grp.Icon != null && grp.Icon.Length > 0) {
          item.Icon = Csc.Components.Imaging.ImageExtensions.LoadImageSource(grp.Icon);
        }
        if (grp.SmallIcon != null && grp.SmallIcon.Length > 0)
          item.SmallIcon = Csc.Components.Imaging.ImageExtensions.LoadImageSource(grp.SmallIcon);
        else if (item.Icon != null)
          item.SmallIcon = item.Icon;

        RadTreeView tree = new RadTreeView();
        tree.ItemsSource = grp.Views.OrderBy(s => s.Order).ThenBy(s => s.Name).ToArray();
        tree.ItemTemplate = this.Resources["ViewDataTemplate"] as DataTemplate;

        tree.Loaded += NavigationTreeView_Loaded;
        tree.ItemClick += NavigationTreeView_ItemClick;

        item.Content = tree;
        this.NavigationOutlookBar.Items.Add(item);
      }

      if (this.NavigationOutlookBar.Items.Count > 0 )
        this.NavigationOutlookBar.SelectedIndex = 0;
    }

    private void NavigationOutlookBar_SelectionChanged(object sender, RadSelectionChangedEventArgs e) {
      //foreach (var itm in e.RemovedItems) {
      //  var tab = (RadOutlookBarItem)itm;
      //  ((RadTreeView)tab.Content).SelectedItem = null;
      //}
    }
    #endregion



    #region Login
    private void OnLogin() {
      LoginWindow wnd = new LoginWindow();
      wnd.Closed += LoginWindow_Closed;
      wnd.Owner = this;
      wnd.ShowDialog();
    }

    private void LoginWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (true == e.DialogResult) {
        OnLoggedOn();
      } else
        this.Close();
    }

    private void OnLoggedOn() {
      SetUserOptions();
      LoadNavigation();
      this.ContentGrid.ShowAnimated();
    }
    #endregion

    #region Navigation
    private void LoadNavigation() {
      // TODO: find better way
      this.ViewRoot.LoadViews(DataManager.CurrentUser.Views);
      LoadNavigationItems(DataManager.CurrentUser.Views);
      //this.ViewRoot.NavigateHome();

    }



    public void NavigateTo(View view) {
      this.ViewRoot.NavigateTo(view);
    }
    #endregion

    #region User Options
    private void SetUserOptions() {
      this.ChangePasswordMenuItem.IsEnabled = DataManager.CurrentUser.LoginMode == LoginStatus.System && DataManager.CurrentUser.CanChangePassword == true;
      TextBlock text = new TextBlock();
      text.Text = string.Format("Hello, {0}", DataManager.CurrentUser.FirstNameOrUsername);
      text.FontSize = Telerik.Windows.Controls.Windows8Palette.Palette.FontSizeXXL;
      text.FontFamily = Telerik.Windows.Controls.Windows8Palette.Palette.FontFamilyLight;
      this.UserDropDownButton.Content = text;
    }

    private void ProfileMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.UserContextMenu.IsOpen = false;

      //ProfileWindow wnd = new ProfileWindow();
      //wnd.Owner = Application.Current.MainWindow;
      //wnd.Closed += (obj, args) => {
      //  if (args.DialogResult == true)
      //    this.RefreshUsername();
      //};
      //wnd.ShowDialog();
    }

    private void ChangePasswordMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.UserContextMenu.IsOpen = false;
      //UpdatePasswordWindow wnd = new UpdatePasswordWindow();
      //wnd.Owner = Application.Current.MainWindow;
      //wnd.ShowDialog();
    }




    #endregion


    #region Signout
    private void SignoutMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      this.UserContextMenu.IsOpen = false;
      OnSignout();
    }

    private void OnSignout() {
      this.SetBusy();
      AccountDataManager.Signout(OnSignedOut);
    }

    private void OnSignedOut(Exception error) {
      if (error != null) {
        this.AlertError(error);
      }
      this.ClearBusy();
    }

    private void AccountDataManager_SignedOut(object sender, System.EventArgs e) {
      try {
        Application.Current.CloseAllWindows(false);
        this.ContentGrid.HideAnimated(() => this.ViewRoot.NavigateNull());
        OnLogin();
      } catch (Exception ex) {
        App.ReportError(ex);
        Application.Current.Shutdown();
      }
    }

    #endregion
  }
}
