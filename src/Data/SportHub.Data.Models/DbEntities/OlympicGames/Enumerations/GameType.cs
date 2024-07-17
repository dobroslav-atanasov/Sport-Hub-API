namespace SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("GameTypes", Schema = "enum")]
public class GameType : BaseTable<int>
{
}