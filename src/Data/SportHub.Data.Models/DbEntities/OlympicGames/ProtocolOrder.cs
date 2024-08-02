namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;
using global::SportHub.Data.Models.Enumerations.OlympicGames;

[Table("ProtocolOrders", Schema = "dbo")]
public class ProtocolOrder : BaseDeletableEntity<Guid>
{
    [Required]
    public GameTypeEnum GameType { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int NOCId { get; set; }

    [Required]
    public string NOCCode { get; set; }

    [Required]
    public int Order { get; set; }
}