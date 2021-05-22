using System.Linq;
using Csc.IntelliSchool.Data;
using System;

namespace Csc.IntelliSchool.Service {
  public partial class SystemService {
    #region HumanResources
    public static HumanResourcesFlagList GetHumanResourcesFlagList() {
      using (var ent = CreateModel())
        return InternalGetHumanResourcesFlagList(ent);
    }

    public static HumanResourcesFlagList InternalGetHumanResourcesFlagList(DataEntities ent) {
      return new HumanResourcesFlagList(InternalGetSystemFlags(ent, SystemFlagsModule.HumanResources, null));
    }
    #endregion


    #region Common
    private static SystemFlag[] InternalGetSystemFlags(DataEntities ent, SystemFlagsModule? module, Enum section) {
      string moduleStr = module != null ? module.Value.ToString().ToLower() : null;
      string sectionStr = section != null ? section.ToString().ToLower() : null;

      var qry = ent.SystemFlags.AsQueryable();

      if (module != null)
        qry = qry.Where(s => s.Module.ToLower() == moduleStr);

      if (section != null)
        qry = qry.Where(s => s.Section.ToLower() == sectionStr);

      return qry.ToArray();
    }
    #endregion
  }
}