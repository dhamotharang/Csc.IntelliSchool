using System;
using System.Linq;
using Csc.Components.Common;
using System.Linq.Expressions;
using Csc.IntelliSchool.Data;
using System.Data.Entity;

namespace Csc.IntelliSchool.WebService.Services.Common {
  public partial class CommonDataService {
    #region Religions
    public Religion[] GetReligions() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<Religion>().OrderBy(s => s.Name).ToArray();
      }
    }
    public Religion AddOrUpdateReligion(Religion item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.ReligionID == 0;

        var itm =  ent.AddOrUpdateItem<Religion>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(), 
          item.ReligionID.PackArray(), 
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return itm;
      }
    }
    public void DeleteReligion(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<Religion>(id);
      }
    }
    #endregion

    #region Nationalities
    public Nationality[] GetNationalities() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<Nationality>().OrderBy(s=>s.Name).ToArray();
      }
    }
    public Nationality AddOrUpdateNationality(Nationality item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.NationalityID == 0;

        item =  ent.AddOrUpdateItem<Nationality>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.NationalityID.PackArray(),
                new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();


        return item;
      }
    }
    public void DeleteNationality(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<Nationality>(id);
      }
    }
    #endregion


  }
}

