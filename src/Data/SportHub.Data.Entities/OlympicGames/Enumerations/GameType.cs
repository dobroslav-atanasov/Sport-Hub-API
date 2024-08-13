namespace SportHub.Data.Entities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("GameTypes", Schema = "enum")]
public class GameType : BaseTable<int>
{
}