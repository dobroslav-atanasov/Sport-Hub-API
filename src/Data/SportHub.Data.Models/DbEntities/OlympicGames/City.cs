namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;

[Table("Cities", Schema = "dbo")]
public class City : BaseDeletableEntity<Guid>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public Guid CountryId { get; set; }
    public virtual Country Country { get; set; }

    public virtual ICollection<Game> Games { get; set; } = [];
}