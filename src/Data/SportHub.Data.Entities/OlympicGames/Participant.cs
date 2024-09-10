namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Participants", Schema = "dbo")]
public class Participant : BaseDeletableEntity<Guid>, IEquatable<Participant>
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [Required]
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }

    [Required]
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    [Required]
    public int NOCId { get; set; }
    public virtual NOC NOC { get; set; }

    public Guid? TeamId { get; set; }
    public virtual Team Team { get; set; }

    public int AgeYears { get; set; }

    public int AgeDays { get; set; }

    public int Medal { get; set; }

    public int Order { get; set; }


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

    public bool Equals(Participant other)
    {
        var equals = true;

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
        return obj is Participant && this.Equals((Participant)obj);
    }
}