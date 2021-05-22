
using Csc.Wpf;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class View : IView {
    // TODO: Fix
    [IgnoreDataMember][NotMapped]
    bool IView.IsHome {
      //get {  return DataManager.CurrentUser.UserViews.Where(s => s.ViewID == this.ViewID && s.IsHome == true).Count() > 0; }
      get; set;
    }

    [IgnoreDataMember][NotMapped]
    string IView.Path
    {
      get
      {
        return this.Path;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    string IView.Assembly {
      get {
        return this.Assembly;
      }
    }

    [IgnoreDataMember][NotMapped]
    string IView.Params
    {
      get { return this.Params; }
    }

    string IView.UserParams
    {
      get; set;
    }

    [IgnoreDataMember][NotMapped]
    IView[] IView.ChildViews
    {
      get
      {
        return this.ChildViews.Select(s => (IView)s).ToArray();
      }
    }
  }

}