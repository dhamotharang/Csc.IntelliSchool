
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public partial class User {
    [IgnoreDataMember][NotMapped]
    public string DisplayName { get { return string.IsNullOrWhiteSpace(FullName) == false ? FullName : Username; } }
    [IgnoreDataMember][NotMapped]
    public string FullDisplayName { get { return string.Format("{0} ({1})", FullName, Username).Trim(); } }
    [IgnoreDataMember][NotMapped]
    public string FullName { get { return string.Format("{0} {1}", FirstName, LastName).Trim(); } }
    [IgnoreDataMember][NotMapped]
    public string FirstNameOrUsername { get { return string.IsNullOrWhiteSpace(FirstName) == false ? FirstName.Trim() : Username; } }

    [IgnoreDataMember][NotMapped]
    public PasswordFormat PasswordFormatTyped
    {
      get
      {
        Data.PasswordFormat format = Data.PasswordFormat.Unknown;
        Enum.TryParse(PasswordFormat, out format);
        return format;
      }
      set { PasswordFormat = value.ToString(); }
    }

    [IgnoreDataMember][NotMapped]
    public LoginStatus LoginMode { get; set; }

    [IgnoreDataMember][NotMapped]
    public View[] Views { get; set; }
  }


}