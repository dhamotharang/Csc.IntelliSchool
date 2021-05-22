using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Csc.Wpf {
  public interface IView {
    bool IsHome { get; set; }
    string Path { get; }
    string Assembly { get; }

    string Params { get; }
    string UserParams { get; set; }
    IView[] ChildViews { get; }
  }

  public class ViewParam {
    public string Param { get; set; }
    public string Value { get; set; }

    public ViewParam() { }
    public ViewParam(string param, string value) { Param = param; Value = value; }
  }
}
