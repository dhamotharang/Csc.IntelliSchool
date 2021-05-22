using Csc.IntelliSchool.Data; using Csc.IntelliSchool.Business;
using System;
using System.Collections.ObjectModel;
using System.Linq; using Csc.Wpf; using Csc.Components.Common; using Csc.Wpf.Data; 
using System.Windows;
using System.Windows.Controls;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
  public partial class EarningInfoControl : Csc.Wpf.UserControlBase {
    public Employee Employee { get { return DataContext as Employee; } }

    public EarningInfoControl() {
      InitializeComponent();
    }

    #region Loading
    private void UserControlBase_Initialized(object sender, System.EventArgs e) {  }
    private void UserControlBase_Loaded(object sender, RoutedEventArgs e) { }
    private void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {}

    public void OnLoadData(bool force = false) {
      throw new NotImplementedException();
    }
    #endregion


  }
}
