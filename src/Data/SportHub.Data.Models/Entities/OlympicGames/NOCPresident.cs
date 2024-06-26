namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("NOCPresidents", Schema = "dbo")]
public class NOCPresident : BaseDeletableEntity<Guid>, IEquatable<NOCPresident>
{
    [MaxLength(500)]
    public string Name { get; set; }

    public int? From { get; set; }

    public int? To { get; set; }

    public int? NOCId { get; set; }
    public virtual NOC NOC { get; set; }

    public bool Equals(NOCPresident other)
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
        return (obj is NOCPresident) && this.Equals((NOCPresident)obj);
    }
}