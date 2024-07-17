namespace SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("FinishTypes", Schema = "enum")]
public class FinishStatus : BaseTable<int>
{
}