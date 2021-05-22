using System.Windows;

namespace Csc.Components.Common.Data {


  public class KeyValue<TKey, TValue> : DynObject {
    public static readonly string Key_Key = FormatColumnName("Key");
    public static readonly string Key_Value = FormatColumnName("Value");

    public TKey Key { get { return this[Key_Key] != null ? (TKey)this[Key_Key] : default(TKey); } set { this[Key_Key] = value; } }
    public TValue Value { get { return this[Key_Value] != null ? (TValue)this[Key_Value] : default(TValue); } set { this[Key_Value] = value; } }


    public KeyValue() { }
    public KeyValue(TKey key) {
      Key = key;
    }
    public KeyValue(TKey key, TValue value) {
      Key = key;
      Value = value;
    }
  }

}