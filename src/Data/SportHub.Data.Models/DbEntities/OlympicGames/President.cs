namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;

[Table("Presidents", Schema = "dbo")]
public class President : BaseDeletableEntity<Guid>, IEquatable<President>
{
    [MaxLength(500)]
    public string Name { get; set; }

    public int? From { get; set; }

    public int? To { get; set; }

    public Guid? NationalOlympicCommitteeId { get; set; }
    public virtual NationalOlympicCommittee NationalOlympicCommittee { get; set; }

    public bool Equals(President other)
    {
        var equals = true;
        if (this.From != other.From)
        {
            other.From = this.From;
            equals = false;
        }

        if (this.To != other.To)
        {
            other.To = this.To;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return (obj is President) && this.Equals((President)obj);
    }
}