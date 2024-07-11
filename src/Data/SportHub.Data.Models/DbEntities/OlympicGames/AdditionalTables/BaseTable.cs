namespace SportHub.Data.Models.DbEntities.OlympicGames.AdditionalTables;

using System.ComponentModel.DataAnnotations;

using SportHub.Data.Common.Models;

public abstract class BaseTable<TKey> : BaseDeletableEntity<TKey>
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}