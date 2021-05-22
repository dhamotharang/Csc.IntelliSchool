
using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public partial class EducationDegree {
    [IgnoreDataMember][NotMapped]
    public string FullName
    {
      get { return string.Format("{0} {1}", Name, ArabicName).Trim(); }
    }
  }

}