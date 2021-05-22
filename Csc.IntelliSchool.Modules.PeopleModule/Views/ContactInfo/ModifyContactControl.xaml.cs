using Csc.IntelliSchool.Data;
using Csc.IntelliSchool.Business;
using System.Linq;
using Csc.Wpf;
using Csc.Components.Common;
using Csc.Wpf.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Csc.IntelliSchool.Modules.PeopleModule.Views.ContactInfo {
  public partial class ModifyContactControl : Csc.Wpf.UserControlBase {
    public Contact Item { get { return DataContext as Contact; } }

    public ModifyContactControl() {
      InitializeComponent();
    }

    private void UserControlBase_Initialized(object sender, System.EventArgs e) {

    }
    #region Numbers
    private void AddNumberButton_Click(object sender, RoutedEventArgs e) {
      OnNewNumberItem();
    }

    private void OnNewNumberItem() {
      NumberModifyWindow wnd = new NumberModifyWindow(Item);
      wnd.Closed += ModifyNumberWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ModifyNumberWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((NumberModifyWindow)sender);
      if (wnd.Result == OperationResult.Add) {
        if (wnd.Item.IsDefault)
          Item.Numbers.ToList().ForEach(s => s.IsDefault = false);
        else if (Item.Numbers.Where(s => s.IsDefault == true).Count() == 0)
          wnd.Item.IsDefault = true;
        Item.Numbers.Add(wnd.Item);
        //this.ItemsGridView.SelectItem(wnd.Item);
      } else if (wnd.Result == OperationResult.Delete)
        Item.Numbers.Remove(wnd.Item);
      else if (wnd.Result == OperationResult.Update) {
        Item.Numbers.Remove(wnd.OriginalItem);
        if (wnd.Item.IsDefault)
          Item.Numbers.ToList().ForEach(s => s.IsDefault = false);
        else if (Item.Numbers.Where(s => s.IsDefault == true).Count() == 0)
          wnd.Item.IsDefault = true;
        Item.Numbers.Add(wnd.Item);
        //this.ItemsGridView.SelectItem(wnd.Item);
      };

      this.NumbersGridView.Rebind();
    }

    private void EditNumberButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnEditNumberItem();
    }

    private void OnEditNumberItem() {
      NumberModifyWindow wnd = new NumberModifyWindow((ContactNumber)this.NumbersGridView.SelectedItem);
      wnd.Closed += ModifyNumberWindow_Closed;
      this.DisplayModal(wnd);
    }

    private void DeleteNumberButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteNumberItem();
    }

    private void OnDeleteNumberItem() {
      Popup.ConfirmDelete(null, () => {
        Item.Numbers.Remove((ContactNumber)this.NumbersGridView.SelectedItem);
        this.NumbersGridView.Rebind();
      });
    }
    #endregion

    #region Addresses
    private void AddAddressButton_Click(object sender, RoutedEventArgs e) {
      OnNewAddressItem();
    }

    private void OnNewAddressItem() {
      AddressModifyWindow wnd = new AddressModifyWindow(Item);
      wnd.Closed += ModifyAddressWindow_Closed;
      this.DisplayModal(wnd);
    }

    void ModifyAddressWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e) {
      if (e.DialogResult != true)
        return;

      var wnd = ((AddressModifyWindow)sender);
      if (wnd.Result == OperationResult.Add) {
        if (wnd.Item.IsDefault)
          Item.Addresses.ToList().ForEach(s => s.IsDefault = false);
        else if (Item.Addresses.Where(s => s.IsDefault == true).Count() == 0)
          wnd.Item.IsDefault = true;
        Item.Addresses.Add(wnd.Item);
        //this.ItemsGridView.SelectItem(wnd.Item);
      } else if (wnd.Result == OperationResult.Delete)
        Item.Addresses.Remove(wnd.Item);
      else if (wnd.Result == OperationResult.Update) {
        Item.Addresses.Remove(wnd.OriginalItem);
        if (wnd.Item.IsDefault)
          Item.Addresses.ToList().ForEach(s => s.IsDefault = false);
        else if (Item.Addresses.Where(s => s.IsDefault == true).Count() == 0)
          wnd.Item.IsDefault = true;
        Item.Addresses.Add(wnd.Item);
        //this.ItemsGridView.SelectItem(wnd.Item);
      };

      this.AddressesGridView.Rebind();
    }

    private void EditAddressButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnEditAddressItem();
    }

    private void OnEditAddressItem() {
      AddressModifyWindow wnd = new AddressModifyWindow((ContactAddress)this.AddressesGridView.SelectedItem);
      wnd.Closed += ModifyAddressWindow_Closed;
      this.DisplayModal(wnd);
    }

    private void DeleteAddressButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow();
      OnDeleteAddressItem();
    }

    private void OnDeleteAddressItem() {
      Popup.ConfirmDelete(null, () => {
        Item.Addresses.Remove((ContactAddress)this.AddressesGridView.SelectedItem);
        this.AddressesGridView.Rebind();
      });
    }
    #endregion



    #region address ContextMenu
    private void AddressesContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();


      menu.FindMenuItem("EditAddressMenuItem").IsEnabled = this.AddressesGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteAddressMenuItem").IsEnabled = this.AddressesGridView.SelectedItem != null;
    }

    private void EditAddressMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditAddressItem();
    }

    private void NewAddressMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnNewAddressItem();
    }

    private void DeleteAddressMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteAddressItem();
    }
    #endregion


    #region Number ContextMenu
    private void NumbersContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      var menu = sender as Telerik.Windows.Controls.RadContextMenu;
      var row = menu.GetClickedRow();

      row?.FocusSelect();



      menu.FindMenuItem("EditNumberMenuItem").IsEnabled = this.NumbersGridView.SelectedItem != null;
      menu.FindMenuItem("DeleteNumberMenuItem").IsEnabled = this.NumbersGridView.SelectedItem != null;
    }

    private void EditNumberMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnEditNumberItem();
    }

    private void NewNumberMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnNewNumberItem();
    }

    private void DeleteNumberMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e) {
      OnDeleteNumberItem();
    }
    #endregion

    private void MenuButton_Click(object sender, RoutedEventArgs e) {
      e.SelectParentRow(); (sender as FrameworkElement).OpenContextMenu();
    }


  }
}
