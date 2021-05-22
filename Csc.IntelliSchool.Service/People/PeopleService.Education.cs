using System;
using System.Linq;
using Csc.Components.Common;
using System.Linq.Expressions;
using Csc.IntelliSchool.Data;
using System.Data.Entity;

namespace Csc.IntelliSchool.Service {
  public partial class PeopleService {
    #region EducationFields
    public EducationField[] GetEducationFields() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EducationField>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EducationField AddOrUpdateEducationField(EducationField item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.FieldID == 0;

        var itm =  ent.AddOrUpdateItem<EducationField>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(), 
          item.FieldID.PackArray(), 
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return itm;
      }
    }
    public void DeleteEducationField(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EducationField>(id);
      }
    }
    #endregion

    #region EducationDegrees
    public EducationDegree[] GetEducationDegrees() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EducationDegree>().OrderBy(s=>s.Name).ToArray();
      }
    }
    public EducationDegree AddOrUpdateEducationDegree(EducationDegree item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.DegreeID == 0;

        item =  ent.AddOrUpdateItem<EducationDegree>(item);

        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.DegreeID.PackArray(),
                new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();


        return item;
      }
    }
    public void DeleteEducationDegree(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EducationDegree>(id);
      }
    }
    #endregion


  }
}

