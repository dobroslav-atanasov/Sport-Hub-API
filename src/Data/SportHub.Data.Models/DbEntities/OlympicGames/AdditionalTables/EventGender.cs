namespace SportHub.Data.Models.DbEntities.OlympicGames.AdditionalTables;

using System.ComponentModel.DataAnnotations.Schema;

[Table("EventGenders", Schema = "dbo")]
public class EventGender : BaseTable<int>
{
}