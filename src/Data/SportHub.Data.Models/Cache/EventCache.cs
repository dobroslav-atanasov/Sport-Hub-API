namespace SportHub.Data.Models.Cache;
public class EventCache /*: IMapFrom<Event>*/
{
    public Guid Id { get; set; }

    public string OriginalName { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public int Gender { get; set; }

    public bool IsTeamEvent { get; set; }

    public int DisciplineId { get; set; }

    public int GameId { get; set; }
}