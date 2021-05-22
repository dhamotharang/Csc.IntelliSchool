
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalProgramInfo", Schema = "HR")]
  public partial class EmployeeMedicalProgramInfo {
    [Key]
    [ForeignKey("Program")]
    public int ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }


    public Nullable<int> Death { get; set; }
    public Nullable<int> Disability { get; set; }
    public Nullable<int> Accidents { get; set; }
    public Nullable<int> Medical { get; set; }
    public string Accomodation { get; set; }
    public Nullable<decimal> InPatient { get; set; }
    public Nullable<decimal> OutPatient { get; set; }
    public Nullable<decimal> Labaratories { get; set; }
    public Nullable<decimal> Radiology { get; set; }
    public Nullable<decimal> Pharmaceutical { get; set; }
    public Nullable<int> PharmaceuticalMax { get; set; }
    public Nullable<decimal> Physiotherapy { get; set; }
    public Nullable<int> PhysiotherapyCount { get; set; }
    public Nullable<decimal> Dental { get; set; }
    public Nullable<int> DentalMax { get; set; }
    public Nullable<decimal> Optical { get; set; }
    public Nullable<int> OpticalMax { get; set; }
    public Nullable<decimal> Pregnancy { get; set; }
    public Nullable<int> PregnancyNormal { get; set; }
    public Nullable<int> PregnancyCesarean { get; set; }
    public Nullable<int> PregnancyLoss { get; set; }
    public Nullable<bool> OutOfNetwork { get; set; }
    public Nullable<int> PreExisting { get; set; }
    public Nullable<int> Chronic { get; set; }
    public Nullable<int> Major { get; set; }
    public Nullable<bool> Pool { get; set; }

  }
}
