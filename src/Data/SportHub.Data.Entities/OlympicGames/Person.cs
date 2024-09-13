namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Persons", Schema = "dbo")]
public class Person : BaseDeletableEntity<Guid>, IEquatable<Person>
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string TvName { get; set; }

    [MaxLength(200)]
    public string ShortName { get; set; }

    [MaxLength(200)]
    public string FirstName { get; set; }

    [MaxLength(200)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(10)]
    public string Gender { get; set; }

    [DataType("DATETIME2")]
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Organisation model
    /// </summary>
    [MaxLength(10)]
    public string Organisation { get; set; }

    /// <summary>
    /// Nationality model
    /// </summary>
    [MaxLength(10)]
    public string Nationality { get; set; }

    public int Height { get; set; }

    public int Weight { get; set; }

    /// <summary>
    /// MainFunction
    /// </summary>
    [Required]
    [MaxLength(5)]
    public string CategoryGroup { get; set; }

    [MaxLength(100)]
    public string CategoryName { get; set; }

    [MaxLength(20)]
    public string CategoryCode { get; set; }

    /// <summary>
    /// Bio
    /// </summary>
    [MaxLength(100)]
    public string PlaceOfBirth { get; set; }

    [MaxLength(100)]
    public string CountryOfBirth { get; set; }

    [MaxLength(100)]
    public string Residence { get; set; }

    [MaxLength(100)]
    public string CountryOfResidence { get; set; }

    /// <summary>
    /// Highlights
    /// </summary>
    [MaxLength(100)]
    public string HighlightsType { get; set; }

    [MaxLength(10000)]
    public string Highlights { get; set; }

    /// <summary>
    /// Interest
    /// </summary>
    [MaxLength(2000)]
    public string Nickname { get; set; }

    [MaxLength(2000)]
    public string Hobbies { get; set; }

    [MaxLength(2000)]
    public string Occupation { get; set; }

    [MaxLength(2000)]
    public string Education { get; set; }

    [MaxLength(2000)]
    public string Family { get; set; }

    [MaxLength(2000)]
    public string LanguagesSpoken { get; set; }

    [MaxLength(2000)]
    public string Coach { get; set; }

    [MaxLength(2000)]
    public string Debut { get; set; }

    [MaxLength(2000)]
    public string Start { get; set; }

    [MaxLength(2000)]
    public string Reason { get; set; }

    [MaxLength(10000)]
    public string Ambition { get; set; }

    [MaxLength(10000)]
    public string Milestones { get; set; }

    [MaxLength(2000)]
    public string Hero { get; set; }

    [MaxLength(2000)]
    public string Influence { get; set; }

    [MaxLength(2000)]
    public string Philosophy { get; set; }

    [MaxLength(10000)]
    public string Award { get; set; }

    [MaxLength(10000)]
    public string AddInformation { get; set; }

    public virtual ICollection<Participant> Participations { get; set; } = new HashSet<Participant>();

    public bool Equals(Person other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.TvName != other.TvName)
        {
            other.TvName = this.TvName;
            equals = false;
        }

        if (this.ShortName != other.ShortName)
        {
            other.ShortName = this.ShortName;
            equals = false;
        }

        if (this.Gender != other.Gender)
        {
            other.Gender = this.Gender;
            equals = false;
        }

        if (this.BirthDate != other.BirthDate)
        {
            other.BirthDate = this.BirthDate;
            equals = false;
        }

        if (this.Height != other.Height)
        {
            other.Height = this.Height;
            equals = false;
        }

        if (this.Weight != other.Weight)
        {
            other.Weight = this.Weight;
            equals = false;
        }

        if (this.FirstName != other.FirstName)
        {
            other.FirstName = this.FirstName;
            equals = false;
        }

        if (this.LastName != other.LastName)
        {
            other.LastName = this.LastName;
            equals = false;
        }

        if (this.Organisation != other.Organisation)
        {
            other.Organisation = this.Organisation;
            equals = false;
        }

        if (this.Nationality != other.Nationality)
        {
            other.Nationality = this.Nationality;
            equals = false;
        }

        if (this.PlaceOfBirth != other.PlaceOfBirth)
        {
            other.PlaceOfBirth = this.PlaceOfBirth;
            equals = false;
        }

        if (this.CountryOfBirth != other.CountryOfBirth)
        {
            other.CountryOfBirth = this.CountryOfBirth;
            equals = false;
        }

        if (this.Residence != other.Residence)
        {
            other.Residence = this.Residence;
            equals = false;
        }

        if (this.CountryOfResidence != other.CountryOfResidence)
        {
            other.CountryOfResidence = this.CountryOfResidence;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Person && this.Equals((Person)obj);
    }
}