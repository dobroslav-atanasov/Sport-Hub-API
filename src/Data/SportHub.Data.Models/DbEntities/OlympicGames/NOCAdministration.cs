namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;
using global::SportHub.Data.Models.Enumerations.OlympicGames;

[Table("NOCAdministrations", Schema = "dbo")]
public class NOCAdministration : BaseDeletableEntity<Guid>
{
    [Required]
    public AdministrationEnum Type { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(10)]
    public string Title { get; set; }

    public int? FromDate { get; set; }

    public int? ToDate { get; set; }

    [Required]
    public int NOCId { get; set; }
    public virtual NOC NOC { get; set; }
}