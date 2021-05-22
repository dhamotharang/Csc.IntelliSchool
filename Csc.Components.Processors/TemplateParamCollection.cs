using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Csc.Components.Processors {
  public class TemplateParamCollection : ICollection, IEnumerable<TemplateParam> {
    private List<TemplateParam> Items { get; set; }
    public int Count { get { return Items.Count; } }
    public bool IsSynchronized { get { return false; } }
    public object SyncRoot { get { return this; } }
    public TemplateParam this[string name] {
      get {
        return Items.SingleOrDefault(s => s.Name.ToLower() == name.ToLower());
      }
    }

    public TemplateParamCollection() { Items = new List<TemplateParam>(); }
    public TemplateParamCollection(IEnumerable<TemplateParam> parameters)
      : this() {
      Items.AddRange(parameters);
    }

    public void Add(TemplateParam param) {
      Items.Add(param);
    }
    public void CopyTo(Array array, int index) {
      foreach (var itm in Items) {
        array.SetValue(itm, index);
        index = index + 1;
      }
    }
    public IEnumerator GetEnumerator() {
      return Items.GetEnumerator();
    }
    IEnumerator<TemplateParam> IEnumerable<TemplateParam>.GetEnumerator() {
      return Items.GetEnumerator();
    }
  }
  
}
