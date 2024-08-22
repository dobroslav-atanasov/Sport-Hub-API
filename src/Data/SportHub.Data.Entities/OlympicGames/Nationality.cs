namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Nationalities", Schema = "dbo")]
public class Nationality : BaseDeletableEntity<int>
{
    [MaxLength(10)]
    public string Code { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string LongName { get; set; }

    public virtual ICollection<Person> Persons { get; set; } = new HashSet<Person>();
}