namespace SportHub.Data.ViewModels.OlympicGames.Games;

using AutoMapper;

using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Services.Mapper.Interfaces;

public class GameListViewModel : IMapFrom<Game>, ICustomMap
{
    public int Id { get; set; }

    public int Year { get; set; }

    public int Type { get; set; }

    public string City { get; set; }

    public string Name { get; set; }

    public void CreateMap(IProfileExpression profile)
    {
        profile.CreateMap<Game, GameListViewModel>()
            .ForMember(x => x.Type, y => y.MapFrom(z => z.Type))
            .ForMember(x => x.Name, y => y.MapFrom(z => z.OfficialName))
            .ForMember(x => x.City, y => y.MapFrom(z => string.Join("/", z.Cities.OrderBy(x => x.Name).Select(x => x.Name))));
    }
}