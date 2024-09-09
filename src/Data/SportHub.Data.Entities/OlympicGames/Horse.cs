namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Horses", Schema = "dbo")]
public class Horse : BaseDeletableEntity<Guid>, IEquatable<Horse>
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string ShortName { get; set; }

    public int YearBirth { get; set; }

    [Required]
    [MaxLength(20)]
    public string OriginalCode { get; set; }

    [MaxLength(10)]
    public string Discipline { get; set; }

    public bool Current { get; set; }

    [MaxLength(200)]
    public string Owner { get; set; }

    [MaxLength(20)]
    public string Organisation { get; set; }

    [MaxLength(100)]
    public string Passport { get; set; }

    [MaxLength(200)]
    public string Sire { get; set; }

    [MaxLength(200)]
    public string Groom { get; set; }

    [MaxLength(200)]
    public string SecondOwner { get; set; }

    [MaxLength(100)]
    public string Colour { get; set; }

    [MaxLength(100)]
    public string BreedCode { get; set; }

    [MaxLength(200)]
    public string Breed { get; set; }

    [MaxLength(100)]
    public string GenderCode { get; set; }

    [MaxLength(200)]
    public string Gender { get; set; }

    [MaxLength(500)]
    public string FormerRider { get; set; }

    [MaxLength(100)]
    public string CountryOfBirth { get; set; }

    [MaxLength(100)]
    public string Info { get; set; }

    [MaxLength(10000)]
    public string Achievements { get; set; }

    public string Meta { get; set; }

    public bool Equals(Horse other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.ShortName != other.ShortName)
        {
            other.ShortName = this.ShortName;
            equals = false;
        }

        if (this.YearBirth != other.YearBirth)
        {
            other.YearBirth = this.YearBirth;
            equals = false;
        }

        if (this.Discipline != other.Discipline)
        {
            other.Discipline = this.Discipline;
            equals = false;
        }

        if (this.Owner != other.Owner)
        {
            other.Owner = this.Owner;
            equals = false;
        }

        if (this.Current != other.Current)
        {
            other.Current = this.Current;
            equals = false;
        }

        if (this.SecondOwner != other.SecondOwner)
        {
            other.SecondOwner = this.SecondOwner;
            equals = false;
        }

        if (this.Organisation != other.Organisation)
        {
            other.Organisation = this.Organisation;
            equals = false;
        }

        if (this.Passport != other.Passport)
        {
            other.Passport = this.Passport;
            equals = false;
        }

        if (this.Sire != other.Sire)
        {
            other.Sire = this.Sire;
            equals = false;
        }

        if (this.Groom != other.Groom)
        {
            other.Groom = this.Groom;
            equals = false;
        }

        if (this.Colour != other.Colour)
        {
            other.Colour = this.Colour;
            equals = false;
        }

        if (this.Breed != other.Breed)
        {
            other.Breed = this.Breed;
            equals = false;
        }

        if (this.BreedCode != other.BreedCode)
        {
            other.BreedCode = this.BreedCode;
            equals = false;
        }

        if (this.Gender != other.Gender)
        {
            other.Gender = this.Gender;
            equals = false;
        }

        if (this.GenderCode != other.GenderCode)
        {
            other.GenderCode = this.GenderCode;
            equals = false;
        }

        if (this.FormerRider != other.FormerRider)
        {
            other.FormerRider = this.FormerRider;
            equals = false;
        }

        if (this.Achievements != other.Achievements)
        {
            other.Achievements = this.Achievements;
            equals = false;
        }

        if (this.CountryOfBirth != other.CountryOfBirth)
        {
            other.CountryOfBirth = this.CountryOfBirth;
            equals = false;
        }

        if (this.Info != other.Info)
        {
            other.Info = this.Info;
            equals = false;
        }

        if (this.Meta != other.Meta)
        {
            other.Meta = this.Meta;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Horse && this.Equals((Horse)obj);
    }
}