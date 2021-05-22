
namespace Csc.Components.Processors {
  public class TemplateParam {
    public string Name { get; set; }
    public virtual string Parameter { get { return Name != null ? "{{" + Name.ToLower() + "}}" : null; } }
    public string Value { get; set; }

    public TemplateParam() { }
    public TemplateParam(string name) { Name = name; }
    public TemplateParam(string name, string value) { Name = name; Value = value; }
  }

}
