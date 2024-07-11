namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;

[Table("Countries", Schema = "dbo")]
public class Country : BaseDeletableEntity<Guid>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    [Required]
    public bool IsIndependent { get; set; } = false;

    [StringLength(2)]
    public string TwoDigitsCode { get; set; }

    [Required]
    [StringLength(10)]
    public string Code { get; set; }

    [MaxLength(100)]
    public string Capital { get; set; }

    [MaxLength(50)]
    public string Continent { get; set; }

    public byte[] Flag { get; set; }

    public virtual ICollection<City> Cities { get; set; } = [];
}