namespace SportHub.Data.Entities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("Genders", Schema = "enum")]
public class Gender : BaseTable<int>
{
}