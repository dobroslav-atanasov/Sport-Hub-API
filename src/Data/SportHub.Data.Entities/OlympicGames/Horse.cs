namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Horses", Schema = "dbo")]
public class Horse : BaseDeletableEntity<Guid>
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string ShortName { get; set; }

    public int YearBirth { get; set; }

    [Required]
    [MaxLength(20)]
    public string OriginalCode { get; set; }

    [MaxLength(10)]
    public string Discipline { get; set; }

    public bool Current { get; set; }

    [MaxLength(200)]
    public string Owner { get; set; }

    [MaxLength(20)]
    public string Organisation { get; set; }

    [MaxLength(100)]
    public string Passport { get; set; }

    [MaxLength(200)]
    public string Sire { get; set; }

    [MaxLength(200)]
    public string Groom { get; set; }

    [MaxLength(200)]
    public string SecondOwner { get; set; }
}