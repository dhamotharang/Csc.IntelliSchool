using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Business {
  public static partial class PeopleDataManager {
    public static List<Nationality>[] NationalityLists { get; private set; }
    public static Nationality[] Nationalities
    {
      get
      {
        if (NationalityLists == null)
          return null;

        return NationalityLists.SelectMany(s => s.OrderBy(x => x.Name)).ToArray();
      }
    }
    public static string[] GenderList = { Gender.Male.ToString(), Gender.Female.ToString() };
    public static string[] MaritalStatusList = { MaritalStatus.Single.ToString(), MaritalStatus.Married.ToString(), MaritalStatus.Divorced.ToString(), MaritalStatus.Widowed.ToString()};
    public static Religion[] Religions { get; private set; }


    #region Nationalities
    public static void GetNationalities(bool forceLoad, AsyncState<Nationality[]> callback) {
      if (forceLoad == false && Nationalities != null) {
        Async.OnCallback(Nationalities, null, callback);
        return;
      }

      NationalityLists = null;
      Async.AsyncCall(() => Service.PeopleService.Instance.GetNationalities() , (res, err) => {
        if (err == null) {
          NationalityLists = new List<Nationality>[] {
            res.Where(s=>s.IsLocal == true).ToList(),
            res.Where(s=>s.IsLocal == false).ToList()
          };
        }
        Async.OnCallback(Nationalities, err, callback);
      });
    }
    public static void DeleteNationality(Nationality item, AsyncState<Nationality> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.DeleteNationality(item.NationalityID ), (err) => {
        if (err == null) {
          foreach (var lst in NationalityLists)
            if (lst.Contains(item))
              lst.Remove(item);
        }
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }
    public static void AddNationality(Nationality item, AsyncState<Nationality> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateNationality(item), (res, err) => {
        var resItem = res as Nationality;
        if (err == null) {
          if (resItem.IsLocal)
            NationalityLists[0].Add(resItem);
          else
            NationalityLists[1].Add(resItem);
        }
        Async.OnCallback(resItem, err, callback);
      });
    }
    public static void UpdateNationality(Nationality item, AsyncState<Nationality> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateNationality(item), (res, err) => {
        var resItem = res as Nationality;
        if (err == null) {
          foreach (var lst in NationalityLists)
            if (lst.Contains(item))
              lst.Remove(item);
          if (resItem.IsLocal)
            NationalityLists[0].Add(resItem);
          else
            NationalityLists[1].Add(resItem);
        }
        Async.OnCallback(resItem, err, callback);
      });
    }

    #endregion

    #region Religions
    public static void GetReligions(bool forceLoad, AsyncState<Religion[]> callback) {
      if (forceLoad == false && Religions != null) {
        Async.OnCallback(Religions, null, callback);
        return;
      }

      Religions = null;
      Async.AsyncCall(() => Service.PeopleService.Instance.GetReligions (), (res, err) => {
        if (err == null) {
          Religions = res.OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(Religions, err, callback);
      });
    }
    public static void DeleteReligion(Religion item, AsyncState<Religion> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.DeleteReligion(item.ReligionID), (err) => {
        if (err == null) {
          Religions = Religions.Except(new[] { item }).ToArray();
        }
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }
    public static void AddReligion(Religion item, AsyncState<Religion> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateReligion(item), (res, err) => {
        if (err == null) {
          Religions = Religions.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    public static void UpdateReligion(Religion item, AsyncState<Religion> callback) {
      Async.AsyncCall(() => Service.PeopleService.Instance.AddOrUpdateReligion(item), (res, err) => {
        if (err == null) {
          Religions = Religions.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }

    #endregion
  }
}