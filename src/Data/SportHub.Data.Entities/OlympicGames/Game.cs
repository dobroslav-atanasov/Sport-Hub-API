namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Games", Schema = "dbo")]
public class Game : BaseDeletableEntity<int>, IEquatable<Game>
{
    [Required]
    public int Year { get; set; }

    [MaxLength(10)]
    public string Number { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    [MaxLength(100)]
    public string Host { get; set; }

    [Required]
    [MaxLength(100)]
    public string Country { get; set; }

    [MaxLength(100)]
    public string CoHost { get; set; }

    [MaxLength(100)]
    public string CoHostCountry { get; set; }

    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? OpeningCeremony { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? ClosingCeremony { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? StartCompetitionDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? EndCompetitionDate { get; set; }

    [MaxLength(500)]
    public string OpenBy { get; set; }

    [MaxLength(5000)]
    public string Torchbearers { get; set; }

    [MaxLength(500)]
    public string AthleteOathBy { get; set; }

    [MaxLength(500)]
    public string JudgeOathBy { get; set; }

    [MaxLength(500)]
    public string CoachOathBy { get; set; }

    [MaxLength(500)]
    public string OlympicFlagBearers { get; set; }

    [MaxLength(50000)]
    public string Description { get; set; }

    [MaxLength(10000)]
    public string BidProcess { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

    public bool Equals(Game other)
    {
        var equals = true;

        if (this.Number != other.Number)
        {
            other.Number = this.Number;
            equals = false;
        }

        if (this.OfficialName != other.OfficialName)
        {
            other.OfficialName = this.OfficialName;
            equals = false;
        }

        if (this.OpeningCeremony != other.OpeningCeremony)
        {
            other.OpeningCeremony = this.OpeningCeremony;
            equals = false;
        }

        if (this.ClosingCeremony != other.ClosingCeremony)
        {
            other.ClosingCeremony = this.ClosingCeremony;
            equals = false;
        }

        if (this.Host != other.Host)
        {
            other.Host = this.Host;
            equals = false;
        }

        if (this.Country != other.Country)
        {
            other.Country = this.Country;
            equals = false;
        }

        if (this.CoHost != other.CoHost)
        {
            other.CoHost = this.CoHost;
            equals = false;
        }

        if (this.CoHostCountry != other.CoHostCountry)
        {
            other.CoHostCountry = this.CoHostCountry;
            equals = false;
        }

        if (this.StartCompetitionDate != other.StartCompetitionDate)
        {
            other.StartCompetitionDate = this.StartCompetitionDate;
            equals = false;
        }

        if (this.EndCompetitionDate != other.EndCompetitionDate)
        {
            other.EndCompetitionDate = this.EndCompetitionDate;
            equals = false;
        }

        if (this.OpenBy != other.OpenBy)
        {
            other.OpenBy = this.OpenBy;
            equals = false;
        }

        if (this.Torchbearers != other.Torchbearers)
        {
            other.Torchbearers = this.Torchbearers;
            equals = false;
        }

        if (this.AthleteOathBy != other.AthleteOathBy)
        {
            other.AthleteOathBy = this.AthleteOathBy;
            equals = false;
        }

        if (this.JudgeOathBy != other.JudgeOathBy)
        {
            other.JudgeOathBy = this.JudgeOathBy;
            equals = false;
        }

        if (this.CoachOathBy != other.CoachOathBy)
        {
            other.CoachOathBy = this.CoachOathBy;
            equals = false;
        }

        if (this.OlympicFlagBearers != other.OlympicFlagBearers)
        {
            other.OlympicFlagBearers = this.OlympicFlagBearers;
            equals = false;
        }

        if (this.Description != other.Description)
        {
            other.Description = this.Description;
            equals = false;
        }

        if (this.BidProcess != other.BidProcess)
        {
            other.BidProcess = this.BidProcess;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Game && this.Equals((Game)obj);
    }
}