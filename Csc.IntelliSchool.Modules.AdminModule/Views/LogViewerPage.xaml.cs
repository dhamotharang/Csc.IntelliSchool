using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Linq;
using Csc.Wpf; using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;

namespace Csc.IntelliSchool.Modules.AdminModule.Views {
  public partial class LogViewerPage : Csc.Wpf.PageBase {
    #region Fields
    private SystemLog[] _items;
    private DateTime? _selectedStartTime;
    private DateTime? _selectedEndTime;
    #endregion

    #region Properties
    public SystemLog[] Items { get { return _items; } set { _items = value; OnPropertyChanged(() => Items); } }
    public DateTime? CurrentStartTime{ get; set; }
    public DateTime? CurrentEndTime{ get; set; }
    public DateTime? SelectedStartTime{ get { return _selectedStartTime; } set { if (_selectedStartTime != value) { _selectedStartTime = value; OnPropertyChanged(() => SelectedStartTime); } } }
    public DateTime? SelectedEndTime{ get { return _selectedEndTime; } set { if (_selectedEndTime != value) { _selectedEndTime = value; OnPropertyChanged(() => SelectedEndTime); } } }
    #endregion

    // Constructors
    public LogViewerPage() {
      InitializeComponent();
      SelectedStartTime = DateTime.Today;
      SelectedEndTime = DateTime.Today.ToDayEnd();
    }

    #region Loading
    private void PageBase_Loaded(object sender, RoutedEventArgs e) {
    }
    private void LoadButton_Click(object sender, RoutedEventArgs e) {       OnLoadData();     }
    private void ReloadButton_Click(object sender, RoutedEventArgs e) { OnLoadData(true); }
    private void ReloadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {  OnLoadData(true); }

    private void OnLoadData(bool reload  = false) {
      if (reload ) {
        SelectedStartTime = CurrentStartTime;
        SelectedEndTime = CurrentEndTime;
      }

      if (this.LoadPanel.Validate(true) == false)
        return;

      this.SetBusy();

      SystemDataManager.GetSystemLog(SelectedStartTime.Value, SelectedEndTime.Value, OnDataLoaded);
    }
    private void OnDataLoaded(SystemLog[] result, Exception error) {
      if (error == null) {
        this.CurrentStartTime = SelectedStartTime;
        this.CurrentEndTime = SelectedEndTime;
        Items = result.OrderBy(s => s.LogID).ToArray();
      } else
        Popup.AlertError(error);

      this.ClearBusy();
    }
    #endregion

    #region Menu
    private void ItemsContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


    }
    #endregion

  }
}
