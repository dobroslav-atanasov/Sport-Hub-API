namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Venues", Schema = "dbo")]
public class Venue : BaseDeletableEntity<int>, IEquatable<Venue>
{
    [Required]
    public int Number { get; set; }

    [Required]
    [MaxLength(500)]
    public string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public string City { get; set; }

    [MaxLength(500)]
    public string EnglishName { get; set; }

    [MaxLength(1000)]
    public string FullName { get; set; }

    public double? LatitudeCoordinate { get; set; }

    public double? LongitudeCoordinate { get; set; }

    public int? OpenedYear { get; set; }

    public int? DemolishedYear { get; set; }

    [MaxLength(100)]
    public string Capacity { get; set; }

    public virtual ICollection<EventVenue> EventsVenues { get; set; } = new HashSet<EventVenue>();

    public bool Equals(Venue other)
    {
        var equals = true;
        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.City != other.City)
        {
            other.City = this.City;
            equals = false;
        }

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.EnglishName != other.EnglishName)
        {
            other.EnglishName = this.EnglishName;
            equals = false;
        }

        if (this.FullName != other.FullName)
        {
            other.FullName = this.FullName;
            equals = false;
        }

        if (this.LatitudeCoordinate != other.LatitudeCoordinate)
        {
            other.LatitudeCoordinate = this.LatitudeCoordinate;
            equals = false;
        }

        if (this.LongitudeCoordinate != other.LongitudeCoordinate)
        {
            other.LongitudeCoordinate = this.LongitudeCoordinate;
            equals = false;
        }

        if (this.OpenedYear != other.OpenedYear)
        {
            other.OpenedYear = this.OpenedYear;
            equals = false;
        }

        if (this.DemolishedYear != other.DemolishedYear)
        {
            other.DemolishedYear = this.DemolishedYear;
            equals = false;
        }

        if (this.Capacity != other.Capacity)
        {
            other.Capacity = this.Capacity;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is Venue) && this.Equals((Venue)obj);
    }
}