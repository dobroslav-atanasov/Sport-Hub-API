namespace SportHub.Data.Entities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("Decisions", Schema = "enum")]
public class Decision : BaseTable<int>
{
}