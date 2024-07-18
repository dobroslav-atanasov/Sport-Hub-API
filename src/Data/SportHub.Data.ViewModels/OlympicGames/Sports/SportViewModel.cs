namespace SportHub.Data.ViewModels.OlympicGames.Sports;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class SportViewModel : IMapFrom<Sport>
{
    public string Name { get; set; }

    public string Code { get; set; }
}