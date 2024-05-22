namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Genders", Schema = "dbo")]
public class Gender : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    public virtual ICollection<Athlete> Athletes { get; set; } = new HashSet<Athlete>();
}