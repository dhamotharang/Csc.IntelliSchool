using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace Csc.IntelliSchool.Modules.AdminModule.Views {
  public partial class LibraryRegisterarPage : Csc.Wpf.PageBase {
    #region Fields
    private ObservableCollection<string> _items;
    #endregion

    #region Properties
    public ObservableCollection<string> Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    #endregion

    // Constructors
    public LibraryRegisterarPage() {
      InitializeComponent();
    }

    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
      //Items = new ObservableCollection<string>(Properties.Settings.Default.Library_List.Cast<string>().ToArray());
    }

    private void ItemsGridView_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e) {
      var lib = e.SelectParentRow<string>();

      Popup.Confirm(null, string.Format(Csc.IntelliSchool.Assets.Resources.Admin.Library_Confirm_Register, lib), () => {
        this.SetBusy();
        ProcessExtensions.RegisterLibrary(FileManager.GetLocalFile(LocalFileType.AppComponents, lib), true, OnRegistered);
      });
    }

    private void OnRegistered(RegisterLibraryResult obj) {
      if (obj.Error != null)
        Popup.AlertError(obj.Error);
      else if (obj.Status != RegisterLibraryStatus.Success)
        Popup.AlertError(string.Format(Csc.IntelliSchool.Assets.Resources.Admin.Library_Register_Failed, this.ItemsGridView.SelectedItem.ToString(), obj.Status.ToString()));
      else
        Popup.Alert(Csc.IntelliSchool.Assets.Resources.Admin.Library_Register_Successful);

      this.ClearBusy();
    }


  }
}
