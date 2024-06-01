namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Games", Schema = "dbo")]
public class Game : BaseDeletableEntity<int>, IEquatable<Game>
{
    [Required]
    public int Year { get; set; }

    [MaxLength(10)]
    public string Number { get; set; }

    [Required]
    public int? OlympicGameTypeId { get; set; }
    public virtual OlympicGameType OlympicGameType { get; set; }

    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? OpeningDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? ClosingDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? StartCompetitionDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? EndCompetitionDate { get; set; }

    //public int ParticipantAthletes { get; set; }

    //public int ParticipantMenAthletes { get; set; }

    //public int ParticipantWomenAthletes { get; set; }

    //public int ParticipantNOCs { get; set; }

    //public int MedalEvents { get; set; }

    //public int MedalDisciplines { get; set; }

    //public int MedalSports { get; set; }

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

    public virtual ICollection<Host> Hosts { get; set; } = new HashSet<Host>();

    public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();

    public bool Equals(Game other)
    {
        var equals = true;
        if (this.OfficialName != other.OfficialName)
        {
            other.OfficialName = this.OfficialName;
            equals = false;
        }

        if (this.OpeningDate != other.OpeningDate)
        {
            other.OpeningDate = this.OpeningDate;
            equals = false;
        }

        if (this.ClosingDate != other.ClosingDate)
        {
            other.ClosingDate = this.ClosingDate;
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