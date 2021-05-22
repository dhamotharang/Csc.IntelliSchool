
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("People", Schema = "Ppl")]
  public partial class Person {

    public Person() {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PersonID { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FamilyName { get; set; }
    public string ArabicFirstName { get; set; }
    public string ArabicMiddleName { get; set; }
    public string ArabicLastName { get; set; }
    public string ArabicFamilyName { get; set; }
    public string Gender { get; set; }
    public string MaritalStatus { get; set; }
    public System.DateTime Birthdate { get; set; }
    public string IDNumber { get; set; }
    public string PhotoUrl { get; set; }


    [ForeignKey("Contact")]
    public Nullable<int> ContactID { get; set; }
    public virtual Contact Contact { get; set; }

    #region Personal
    [ForeignKey("Nationality")]
    public Nullable<int> NationalityID { get; set; }
    public virtual Nationality Nationality { get; set; }

    [ForeignKey("Religion")]
    public Nullable<int> ReligionID { get; set; }
    public virtual Religion Religion { get; set; }
    #endregion

    #region Education
    [ForeignKey("EducationDegree")]
    public Nullable<int> EducationDegreeID { get; set; }
    public virtual EducationDegree EducationDegree { get; set; }

    [ForeignKey("EducationField")]
    public Nullable<int> EducationFieldID { get; set; }
    public virtual EducationField EducationField { get; set; }
    #endregion
  }
}
