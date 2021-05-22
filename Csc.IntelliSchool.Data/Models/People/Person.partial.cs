using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Runtime.Serialization;
using Csc.Components.Common;

namespace Csc.IntelliSchool.Data {

  public partial class Person {
    #region Personal
    [IgnoreDataMember]
    [NotMapped]
    public string FullName {
      get {
        return string.Format("{0} {1} {2} {3}", FirstName, MiddleName, LastName, FamilyName).Trim().Replace("  ", " ").Replace("  ", " ");
      }
    }

  

 
    public string ArabicFullName {
      get {
        return string.Format("{0} {1} {2} {3}", ArabicFirstName, ArabicMiddleName, ArabicLastName, ArabicFamilyName).Trim().Replace("  ", " ").Replace("  ", " ");
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int Age {
      get {
        return (int)DateTimeExtensions.CalculatePeriod(Birthdate).Years;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public bool? IsLocal {
      get {
        return this.Nationality != null ? this.Nationality.IsLocal : new bool?();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public Gender GenderTyped {
      get {
        Gender gen = Data.Gender.Unknown;
        if (string.IsNullOrEmpty(Gender) == false)
          Enum.TryParse<Gender>(Gender, out gen);
        return gen;
      }
      set {
        if (value == Data.Gender.Unknown)
          Gender = null;
        else
          Gender = value.ToString();
      }
    }


    [IgnoreDataMember]
    [NotMapped]
    public string FullPhotoUrl {
      get {
        return null;
        //return AppPath.FormatPath(AppPathSection.HumanResourcesPhotos, PhotoUrl);
      }
    }

    #endregion Personal
  }
}
