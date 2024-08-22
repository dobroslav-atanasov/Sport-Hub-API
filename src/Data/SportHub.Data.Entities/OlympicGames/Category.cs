namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Categories", Schema = "dbo")]
public class Category : BaseDeletableEntity<int>
{
    [MaxLength(20)]
    public string Code { get; set; }

    [MaxLength(10)]
    public string Group { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    public virtual ICollection<Person> Persons { get; set; } = new HashSet<Person>();
}