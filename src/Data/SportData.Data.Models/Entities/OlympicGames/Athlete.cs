namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Athletes", Schema = "dbo")]
public class Athlete : BaseDeletableEntity<Guid>, IEquatable<Athlete>
{
    public int Code { get; set; }

    public int? GenderId { get; set; }
    public virtual Gender Gender { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } // Used name

    [Required]
    [MaxLength(200)]
    public string TranslateName { get; set; }

    [MaxLength(200)]
    public string FullName { get; set; }

    [MaxLength(500)]
    public string OriginalName { get; set; }

    [MaxLength(100)]
    public string Citizenship { get; set; } // Nationality

    public DateTime? BirthDate { get; set; }

    [MaxLength(100)]
    public string BirthCity { get; set; }

    [MaxLength(100)]
    public string BirthCountry { get; set; }

    public DateTime? DiedDate { get; set; }

    [MaxLength(100)]
    public string DiedCity { get; set; }

    [MaxLength(100)]
    public string DiedCountry { get; set; }

    public int? HeightInCentimeters { get; set; }

    public double? HeightInInches { get; set; }

    public int? WeightInKilograms { get; set; }

    public int? WeightInPounds { get; set; }

    [MaxLength(10000)]
    public string Description { get; set; }

    public virtual ICollection<AthleteClub> AthletesClubs { get; set; } = new HashSet<AthleteClub>();

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

    public bool Equals(Athlete other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.TranslateName != other.TranslateName)
        {
            other.TranslateName = this.TranslateName;
            equals = false;
        }

        if (this.FullName != other.FullName)
        {
            other.FullName = this.FullName;
            equals = false;
        }

        if (this.GenderId != other.GenderId)
        {
            other.GenderId = this.GenderId;
            equals = false;
        }

        if (this.OriginalName != other.OriginalName)
        {
            other.OriginalName = this.OriginalName;
            equals = false;
        }

        if (this.Citizenship != other.Citizenship)
        {
            other.Citizenship = this.Citizenship;
            equals = false;
        }

        if (this.BirthDate != other.BirthDate)
        {
            other.BirthDate = this.BirthDate;
            equals = false;
        }

        if (this.DiedDate != other.DiedDate)
        {
            other.DiedDate = this.DiedDate;
            equals = false;
        }

        if (this.BirthCity != other.BirthCity)
        {
            other.BirthCity = this.BirthCity;
            equals = false;
        }

        if (this.BirthCountry != other.BirthCountry)
        {
            other.BirthCountry = this.BirthCountry;
            equals = false;
        }

        if (this.DiedCity != other.DiedCity)
        {
            other.DiedCity = this.DiedCity;
            equals = false;
        }

        if (this.DiedCountry != other.DiedCountry)
        {
            other.DiedCountry = this.DiedCountry;
            equals = false;
        }

        if (this.HeightInCentimeters != other.HeightInCentimeters)
        {
            other.HeightInCentimeters = this.HeightInCentimeters;
            equals = false;
        }

        if (this.HeightInInches != other.HeightInInches)
        {
            other.HeightInInches = this.HeightInInches;
            equals = false;
        }

        if (this.WeightInKilograms != other.WeightInKilograms)
        {
            other.WeightInKilograms = this.WeightInKilograms;
            equals = false;
        }

        if (this.WeightInPounds != other.WeightInPounds)
        {
            other.WeightInPounds = this.WeightInPounds;
            equals = false;
        }

        if (this.Description != this.Description)
        {
            other.Description = this.Description;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is Athlete) && this.Equals((Athlete)obj);
    }
}