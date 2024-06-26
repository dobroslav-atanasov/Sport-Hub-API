﻿namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Discpilines", Schema = "dbo")]
public class Discipline : BaseDeletableEntity<int>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(3)]
    public string Code { get; set; }

    public int? SportId { get; set; }
    public virtual Sport Sport { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
}