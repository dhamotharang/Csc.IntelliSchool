using Csc.Components.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Csc.Wpf {
  [Flags]
  public enum FileType {
    Any = 0,
    Excel = 1 << 1,
    ExcelML = 1 << 2,
    CSV = 1 << 3,
    PDF = 1 << 4,
    HTML = 1 << 5,
    Text = 1 << 6,
    PNG = 1 << 7,
    JPEG = 1 << 8,

    Images = PNG | JPEG,
    GridExport = Excel | ExcelML | CSV | HTML | Text | PDF,
  }
}
