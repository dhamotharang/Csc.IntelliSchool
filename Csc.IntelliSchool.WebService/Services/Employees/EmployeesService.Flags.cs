using System.Linq;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    private static HumanResourcesFlagList InternalGetHumanResourcesFlags(DataEntities ent /*, string section */ ) {
      return new HumanResourcesFlagList(InternalGetSystemFlags(ent, HumanResourcesFlagList.ModuleHumanResources, null));
    }

    private static SystemFlag[] InternalGetSystemFlags(DataEntities ent, string module, string section) {
      if (module != null)
        module = module.ToLower();
      if (section != null)
        section = section.ToLower();

      var qry = ent.SystemFlags.AsQueryable();

      if (module != null)
        qry = qry.Where(s => s.Module.ToLower() == module);

      if (section != null)
        qry = qry.Where(s => s.Section.ToLower() == section);


      return qry.ToArray();
    }
  }
}