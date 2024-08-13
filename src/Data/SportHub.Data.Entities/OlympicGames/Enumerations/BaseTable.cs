namespace SportHub.Data.Entities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations;

using global::SportHub.Data.Common.Models;

public abstract class BaseTable<TKey> : BaseDeletableEntity<TKey>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}