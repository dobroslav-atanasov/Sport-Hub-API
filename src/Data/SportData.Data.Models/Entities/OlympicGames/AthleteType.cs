namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("AthleteTypes", Schema = "dbo")]
public class AthleteType : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();
}