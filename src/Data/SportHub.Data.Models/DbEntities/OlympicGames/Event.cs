namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;
using SportHub.Data.Models.Enumerations.OlympicGames;

[Table("Events", Schema = "dbo")]
public class Event : BaseDeletableEntity<Guid>, IEquatable<Event>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string OriginalName { get; set; }

    [Required]
    [MaxLength(200)]
    public string NormalizedName { get; set; }

    [Required]
    public EventGenderEnum Gender { get; set; }

    [Required]
    public int OriginalResultId { get; set; }

    [Required]
    public int GameId { get; set; }
    public virtual Game Game { get; set; }

    [Required]
    public int? DisciplineId { get; set; }
    public virtual Discipline Discipline { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public bool IsTeamEvent { get; set; } = false;

    [MaxLength(200)]
    public string AdditionalInfo { get; set; }

    public string Format { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Venue> Venues { get; set; } = new HashSet<Venue>();

    public bool Equals(Event other)
    {
        var equals = true;
        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.NormalizedName != other.NormalizedName)
        {
            other.NormalizedName = this.NormalizedName;
            equals = false;
        }

        if (this.StartDate != other.StartDate)
        {
            other.StartDate = this.StartDate;
            equals = false;
        }

        if (this.EndDate != other.EndDate)
        {
            other.EndDate = this.EndDate;
            equals = false;
        }

        if (this.IsTeamEvent != other.IsTeamEvent)
        {
            other.IsTeamEvent = this.IsTeamEvent;
            equals = false;
        }

        if (this.AdditionalInfo != other.AdditionalInfo)
        {
            other.AdditionalInfo = this.AdditionalInfo;
            equals = false;
        }

        if (this.Format != other.Format)
        {
            other.Format = this.Format;
            equals = false;
        }

        if (this.Description != other.Description)
        {
            other.Description = this.Description;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is Event) && this.Equals((Event)obj);
    }
}