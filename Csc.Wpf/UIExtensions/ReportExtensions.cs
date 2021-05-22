using Csc.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using Telerik.Reporting.Processing;

namespace Csc.Wpf {
  public static partial class ReportExtensions {
    #region Find Processing Element
    public static object FindElement(Telerik.Reporting.ReportItemBase item, object parent) {
      return Telerik.Reporting.Processing.ElementTreeHelper.FindChildByName(parent as LayoutElement,
        item.Name, true).FirstOrDefault();
    }
    public static T FindElement<T>(this Telerik.Reporting.ReportItemBase item, object parent) where T : LayoutElement {
      return (T)FindElement(item, parent);
    }
    public static TextBox FindElement(this Telerik.Reporting.TextBox item, object parent) {
      return FindElement<TextBox>(item, parent);
    }
    public static Table FindElement(this Telerik.Reporting.Table item, object parent) {
      return FindElement<Table>(item, parent);
    }
    #endregion


    #region Data Object
    public static object GetDataObject(this Telerik.Reporting.Report report, object element) {
      return (element as ProcessingElement).DataObject.RawData;
    }
    public static T GetDataObject<T>(this Telerik.Reporting.Report report, object element) where T : class {
      return GetDataObject(report, element) as T;
    }
    #endregion

    #region Data Source
    /// <summary>
    /// Sets data source to the item as the data object of the parent
    /// </summary>
    public static void SetDataSource(this Telerik.Reporting.ReportItemBase item, object parent) {
      SetDataSource(item, parent, GetDataObject(item.Report, parent));
    }
    public static void SetDataSource(this Telerik.Reporting.ReportItemBase item, object parent, object ds) {
      FindElement<Table>(item, parent).DataSource = ds;
    }

    public static void SetValue(this Telerik.Reporting.ReportItemBase item, object parent, object value) {
      FindElement<TextBox>(item, parent).Value = value;
    }
    #endregion

  }
}