using System;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class SystemFlagList {
    public SystemFlag[] Flags { get; private set; }

    public object GetValue(SystemFlagsModule module, Enum section, string name) {
      string moduleStr = module.ToString().ToLower();
      string sectionStr = section.ToString().ToLower();
      name = name.ToLower();

      var flg = Flags.SingleOrDefault(s => s.Module.ToLower() == moduleStr && s.Section.ToLower() == sectionStr && s.Name.ToLower() == name);

      return flg != null ? flg.Value : null;
    }

    public SystemFlagList(SystemFlag[] flags) {
      Flags = flags;
    }
  }



}