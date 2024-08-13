namespace SportHub.Data.Entities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("Medals", Schema = "enum")]
public class Medal : BaseTable<int>
{
}