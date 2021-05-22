using System;
using System.Linq;
using Csc.Components.Common;
using System.Linq.Expressions;
using Csc.IntelliSchool.Data;
using System.Data.Entity;

namespace Csc.IntelliSchool.Service {
  public partial class PeopleService {
    #region Religions
    public Religion[] GetReligions() {
      using (var ent = CreateModel()) {
        return ent.GetItems<Religion>().OrderBy(s => s.Name).ToArray();
      }
    }
    public Religion AddOrUpdateReligion(Religion item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.ReligionID == 0;

        var itm = ent.AddOrUpdateItem<Religion>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.ReligionID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return itm;
      }
    }
    public void DeleteReligion(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<Religion>(id);
      }
    }
    #endregion

    #region Nationalities
    public Nationality[] GetNationalities() {
      using (var ent = CreateModel()) {
        return ent.GetItems<Nationality>().OrderBy(s => s.Name).ToArray();
      }
    }
    public Nationality AddOrUpdateNationality(Nationality item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.NationalityID == 0;

        item = ent.AddOrUpdateItem<Nationality>(item);

        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.NationalityID.PackArray(),
                new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();


        return item;
      }
    }
    public void DeleteNationality(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<Nationality>(id);
      }
    }
    #endregion


  }
}

