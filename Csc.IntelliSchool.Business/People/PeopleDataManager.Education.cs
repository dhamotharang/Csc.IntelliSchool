using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Business {
  public static partial class PeopleDataManager {
    private static EducationField[] EducationFields {
      get { return DataManager.Cache.Get<EducationField[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EducationField[]>();
        else
          DataManager.Cache.Add(value);
      }
    }
    private static EducationDegree[] EducationDegrees {
      get { return DataManager.Cache.Get<EducationDegree[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EducationDegree[]>();
        else
          DataManager.Cache.Add(value);
      }
    }


    #region EducationDegrees
    public static void GetEducationDegrees(bool forceLoad, AsyncState<EducationDegree[]> callback) {
      if (forceLoad == false && EducationDegrees != null) {
        Async.OnCallback(EducationDegrees, null, callback);
        return;
      }

      EducationDegrees = null;
      Async.AsyncCall(() => Service.PeopleService.Instance.GetEducationDegrees(), (res, err) => {
        if (err == null) {
          EducationDegrees = res.OrderBy(s => s.Order).ThenBy(s=>s.Name).ToArray();
        }
        Async.OnCallback(EducationDegrees, err, callback);
      });
    }
    public static void DeleteEducationDegree(EducationDegree item, AsyncState<EducationDegree> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.DeleteEducationDegree(item.DegreeID), (err) => {
        if (err == null) {
          EducationDegrees = EducationDegrees.Except(new[] { item }).ToArray();
        }
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }
    public static void AddEducationDegree(EducationDegree item, AsyncState<EducationDegree> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateEducationDegree(item), (res, err) => {
        if (err == null) {
          EducationDegrees = EducationDegrees.Concat(new[] { res }).OrderBy(s => s.Order).ThenBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    public static void UpdateEducationDegree(EducationDegree item, AsyncState<EducationDegree> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateEducationDegree(item), (res, err) => {
        if (err == null) {
          EducationDegrees = EducationDegrees.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Order).ThenBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }

    #endregion

    #region EducationFields
    public static void GetEducationFields(bool forceLoad, AsyncState<EducationField[]> callback) {
      if (forceLoad == false && EducationFields != null) {
        Async.OnCallback(EducationFields, null, callback);
        return;
      }

      EducationFields = null;
      Async.AsyncCall(() => Service.PeopleService.Instance.GetEducationFields (), (res, err) => {
        if (err == null) {
          EducationFields = res.OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(EducationFields, err, callback);
      });
    }
    public static void DeleteEducationField(EducationField item, AsyncState<EducationField> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.DeleteEducationField(item.FieldID), (err) => {
        if (err == null) {
          EducationFields = EducationFields.Except(new[] { item }).ToArray();
        }
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }
    public static void AddEducationField(EducationField item, AsyncState<EducationField> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateEducationField(item), (res, err) => {
        if (err == null) {
          EducationFields = EducationFields.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    public static void UpdateEducationField(EducationField item, AsyncState<EducationField> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateEducationField(item), (res, err) => {
        if (err == null) {
          EducationFields = EducationFields.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }

    #endregion
  }
}