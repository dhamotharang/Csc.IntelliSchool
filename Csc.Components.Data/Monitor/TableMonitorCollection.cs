using System;
using System.Linq;

namespace Csc.Components.Data {
  public partial class TableMonitorCollection : System.Collections.ObjectModel.KeyedCollection<Type, TableMonitorBase> {
    public event TableChangeEventHandler Changed;


    public void StartAllItems() {
      foreach (var itm in this.Items) {
        StartItem(itm);
      }
    }
    public void StartItem(TableMonitorBase itm) {
      itm.Start();
    }
    public void StartItem(Type key) {
      this[key].Start();
    }

    public void StopAllItems() {
      foreach (var itm in this.Items) {
        StopItem(itm);
      }
    }
    public void StopItem(TableMonitorBase itm) {
      itm.Stop();
    }
    public void StopItem(Type key) {
      this[key].Stop();
    }

    protected override Type GetKeyForItem(TableMonitorBase item) {
      return item.Type;
    }

    protected override void ClearItems() {
      StopAllItems();
      base.ClearItems();
    }


    protected override void InsertItem(int index, TableMonitorBase item) {
      base.InsertItem(index, item);
      item.TableChanged += Item_TableChanged; ;
    }


    protected override void RemoveItem(int index) {
      var item = this.ElementAt(index);
      StopItem(item);
      item.TableChanged -= Item_TableChanged; ;

      base.RemoveItem(index);
    }

    protected override void SetItem(int index, TableMonitorBase item) {
      var oldItem = this.ElementAt(index);
      StopItem(oldItem);
      oldItem.TableChanged -= Item_TableChanged;

      base.SetItem(index, item);
      item.TableChanged += Item_TableChanged;
    }


    private void Item_TableChanged(object sender, TableChangeEventArgs e) {
      Changed?.Invoke(sender, e);
    }
  }
}

