using System.Data.Entity;

namespace Csc.IntelliSchool.Data {
  public partial class DataEntities {
    public virtual DbSet<Person> People { get; set; }

    #region Contact
    public virtual DbSet<Contact> Contacts { get; set; }
    public virtual DbSet<ContactAddress> ContactAddresses { get; set; }
    public virtual DbSet<ContactNumber> ContactNumbers { get; set; }
    public virtual DbSet<ContactEmail> ContactEmails { get; set; }
    #endregion

    #region Personal
    public virtual DbSet<Nationality> Nationalities { get; set; }
    public virtual DbSet<Religion> Religions { get; set; }
    #endregion


    #region Education
    public virtual DbSet<EducationDegree> EducationDegrees { get; set; }
    public virtual DbSet<EducationField> EducationFields { get; set; }
    #endregion

  }

}