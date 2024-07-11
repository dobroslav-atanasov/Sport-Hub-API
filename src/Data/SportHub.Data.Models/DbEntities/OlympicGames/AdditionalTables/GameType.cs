namespace SportHub.Data.Models.DbEntities.OlympicGames.AdditionalTables;

using System.ComponentModel.DataAnnotations.Schema;

[Table("GameTypes", Schema = "dbo")]
public class GameType : BaseTable<int>
{
}