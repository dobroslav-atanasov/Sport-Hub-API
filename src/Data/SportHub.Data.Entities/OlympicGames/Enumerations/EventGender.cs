namespace SportHub.Data.Entities.OlympicGames.Enumerations;

using System.ComponentModel.DataAnnotations.Schema;

[Table("EventGenders", Schema = "enum")]
public class EventGender : BaseTable<int>
{
}