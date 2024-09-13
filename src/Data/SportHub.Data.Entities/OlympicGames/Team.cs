namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Teams", Schema = "dbo")]
public class Team : BaseDeletableEntity<Guid>, IEquatable<Team>
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string TvName { get; set; }

    [MaxLength(200)]
    public string ShortName { get; set; }

    [Required]
    [StringLength(1)]
    public string Gender { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    [MaxLength(10)]
    public string Organisation { get; set; }

    [MaxLength(50)]
    public string OriginalCode { get; set; }

    public int Medal { get; set; }

    [MaxLength(100)]
    public string HighlightsType { get; set; }

    [MaxLength(10000)]
    public string Highlights { get; set; }

    [MaxLength(10000)]
    public string AddInformation { get; set; }

    [MaxLength(100)]
    public string Rank { get; set; }

    [MaxLength(100)]
    public string Played { get; set; }

    [MaxLength(100)]
    public string Wins { get; set; }

    [MaxLength(100)]
    public string Losses { get; set; }

    [MaxLength(100)]
    public string Irm { get; set; }

    public int RankOrder { get; set; }

    [MaxLength(100)]
    public string RankType { get; set; }

    [MaxLength(100)]
    public string RankEqual { get; set; }

    [MaxLength(100)]
    public string ResultType { get; set; }

    [MaxLength(100)]
    public string RankResult { get; set; }

    [MaxLength(100)]
    public string Draws { get; set; }

    public virtual ICollection<Participant> Athletes { get; set; } = new HashSet<Participant>();

    public virtual ICollection<Coach> Coaches { get; set; } = new HashSet<Coach>();

    public bool Equals(Team other)
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

        if (this.Type != other.Type)
        {
            other.Type = this.Type;
            equals = false;
        }

        if (this.Organisation != other.Organisation)
        {
            other.Organisation = this.Organisation;
            equals = false;
        }

        if (this.Rank != other.Rank)
        {
            other.Rank = this.Rank;
            equals = false;
        }

        if (this.Played != other.Played)
        {
            other.Played = this.Played;
            equals = false;
        }

        if (this.Wins != other.Wins)
        {
            other.Wins = this.Wins;
            equals = false;
        }

        if (this.Losses != other.Losses)
        {
            other.Losses = this.Losses;
            equals = false;
        }

        if (this.Irm != other.Irm)
        {
            other.Irm = this.Irm;
            equals = false;
        }

        if (this.RankOrder != other.RankOrder)
        {
            other.RankOrder = this.RankOrder;
            equals = false;
        }

        if (this.RankType != other.RankType)
        {
            other.RankType = this.RankType;
            equals = false;
        }

        if (this.RankEqual != other.RankEqual)
        {
            other.RankEqual = this.RankEqual;
            equals = false;
        }

        if (this.ResultType != other.ResultType)
        {
            other.ResultType = this.ResultType;
            equals = false;
        }

        if (this.RankResult != other.RankResult)
        {
            other.RankResult = this.RankResult;
            equals = false;
        }

        if (this.Draws != other.Draws)
        {
            other.Draws = this.Draws;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Team && this.Equals((Team)obj);
    }
}