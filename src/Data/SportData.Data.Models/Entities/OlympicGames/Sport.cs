namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Models;

[Table("Sports", Schema = "dbo")]
public class Sport : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(2)]
    public string Code { get; set; }

    [Required]
    public int? OlympicGameTypeId { get; set; }
    public virtual OlympicGameType OlympicGameType { get; set; }

    public virtual ICollection<Discipline> Disciplines { get; set; } = new HashSet<Discipline>();
}