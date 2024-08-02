namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;
using global::SportHub.Data.Models.Enumerations.OlympicGames;

[Table("FlagBearers", Schema = "dbo")]
public class FlagBearer : BaseDeletableEntity<Guid>
{
    [Required]
    public GameTypeEnum GameType { get; set; }

    [Required]
    public CeremonyEnum Ceremony { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public Guid AthleteId { get; set; }

    [Required]
    public int NOCId { get; set; }
    public virtual NOC NOC { get; set; }
}