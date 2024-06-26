namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("EventGenderTypes", Schema = "dbo")]
public class EventGenderType : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
}