namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Results", Schema = "dbo")]
public class Result : BaseDeletableEntity<Guid>, IEquatable<Result>
{
    public Guid? EventId { get; set; }
    public virtual Event Event { get; set; }

    public int? GameId { get; set; }
    public virtual Game Game { get; set; }

    [Required]
    public string UniqueNumber { get; set; }

    [Required]
    public string Json { get; set; }

    public virtual ICollection<Participation> Participations { get; set; } = new HashSet<Participation>();

    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();

    public bool Equals(Result other)
    {
        var equals = true;

        if (this.Json != other.Json)
        {
            other.Json = this.Json;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Result && this.Equals((Result)obj);
    }
}