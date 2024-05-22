namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Cities", Schema = "dbo")]
public class City : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public virtual ICollection<Host> Hosts { get; set; } = new HashSet<Host>();
}