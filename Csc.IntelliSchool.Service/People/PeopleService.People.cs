using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class PeopleService : DataService {

    internal void InternalAddOrUpdatePerson(DataEntities ent, Person userItem) {
      if (userItem == null)
        return;

      var dbItem = ent.People.Query(PersonIncludes.All).SingleOrDefault(s=>s.PersonID == userItem.PersonID);

      if (dbItem != null) { // update
        ent.Entry(dbItem).CurrentValues.SetValues(userItem);

        //dbItem.Nationality = userItem.Nationality;
        //ent.UpdateRelatedEntity(dbItem.Nationality, userItem.Nationality);

        //dbItem.Religion = userItem.Religion;
        //ent.UpdateRelatedEntity(dbItem.Religion, userItem.Religion);

        ent.UpdateRelatedEntity(dbItem.Contact, userItem.Contact);
        if (dbItem.Contact != null && userItem.Contact != null) {
          ent.UpdateChildEntities(dbItem.Contact.Numbers.ToArray(), userItem.Contact.Numbers.ToArray(), (a, b) => a.NumberID == b.NumberID);
          ent.UpdateChildEntities(dbItem.Contact.Addresses.ToArray(), userItem.Contact.Addresses.ToArray(), (a, b) => a.AddressID == b.AddressID);
        }
      } else
        ent.People.Add(userItem);
    }

  }

}