namespace SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("Medals", Schema = "enum")]
public class Medal : BaseTable<int>
{
}