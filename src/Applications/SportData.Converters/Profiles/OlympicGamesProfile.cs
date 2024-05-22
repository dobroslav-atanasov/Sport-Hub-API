﻿namespace SportData.Converters.Profiles;

using AutoMapper;

using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Converters.OlympicGames.Base;
using SportData.Data.Models.Converters.OlympicGames.Disciplines;

public class OlympicGamesProfile : Profile
{
    public OlympicGamesProfile()
    {
        this.CreateMap<MatchModel, TeamMatch<Basketball>>()
            .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<AlpineSkiing>>()
            .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<Archery>>()
            .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ReverseMap();

        this.CreateMap<MatchModel, AthleteMatch<Archery>>()
            .ForPath(x => x.Athlete1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Athlete1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Athlete1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Athlete1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Athlete1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Athlete2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Athlete2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Athlete2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Athlete2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Athlete2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<Badminton>>()
           .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
           .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
           .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
           .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
           .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
           .ForPath(x => x.Team1.Seed, opt => opt.MapFrom(y => y.Team1.Seed))
           .ForPath(x => x.Team1.Game1, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(0)))
           .ForPath(x => x.Team1.Game2, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(1)))
           .ForPath(x => x.Team1.Game3, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(2)))
           .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
           .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
           .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
           .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
           .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
           .ForPath(x => x.Team2.Seed, opt => opt.MapFrom(y => y.Team2.Seed))
           .ForPath(x => x.Team2.Game1, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(0)))
           .ForPath(x => x.Team2.Game2, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(1)))
           .ForPath(x => x.Team2.Game3, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(2)))
           .ReverseMap();

        this.CreateMap<MatchModel, AthleteMatch<Badminton>>()
            .ForPath(x => x.Athlete1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Athlete1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Athlete1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Athlete1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Athlete1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Athlete1.Seed, opt => opt.MapFrom(y => y.Team1.Seed))
            .ForPath(x => x.Athlete1.Game1, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(0)))
            .ForPath(x => x.Athlete1.Game2, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(1)))
            .ForPath(x => x.Athlete1.Game3, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(2)))
            .ForPath(x => x.Athlete2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Athlete2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Athlete2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Athlete2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Athlete2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ForPath(x => x.Athlete2.Seed, opt => opt.MapFrom(y => y.Team2.Seed))
            .ForPath(x => x.Athlete2.Game1, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(0)))
            .ForPath(x => x.Athlete2.Game2, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(1)))
            .ForPath(x => x.Athlete2.Game3, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(2)))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<Baseball>>()
            .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Team1.Runs, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Team2.Runs, opt => opt.MapFrom(y => y.Team2.Points))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<BeachVolleyball>>()
           .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
           .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
           .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
           .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
           .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
           .ForPath(x => x.Team1.Seed, opt => opt.MapFrom(y => y.Team1.Seed))
           .ForPath(x => x.Team1.Set1, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(0)))
           .ForPath(x => x.Team1.Set2, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(1)))
           .ForPath(x => x.Team1.Set3, opt => opt.MapFrom(y => y.Team1.Parts.ElementAtOrDefault(2)))
           .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
           .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
           .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
           .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
           .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
           .ForPath(x => x.Team2.Seed, opt => opt.MapFrom(y => y.Team2.Seed))
           .ForPath(x => x.Team2.Set1, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(0)))
           .ForPath(x => x.Team2.Set2, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(1)))
           .ForPath(x => x.Team2.Set3, opt => opt.MapFrom(y => y.Team2.Parts.ElementAtOrDefault(2)))
           .ReverseMap();

        this.CreateMap<MatchModel, AthleteMatch<Boxing>>()
            .ForPath(x => x.Athlete1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Athlete1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Athlete1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Athlete1.Code, opt => opt.MapFrom(y => y.Team1.Code))
            .ForPath(x => x.Athlete2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Athlete2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Athlete2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Athlete2.Code, opt => opt.MapFrom(y => y.Team2.Code))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<Cricket>>()
            .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<Curling>>()
            .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ReverseMap();

        this.CreateMap<MatchModel, AthleteMatch<Fencing>>()
            .ForPath(x => x.Athlete1.Id, opt => opt.MapFrom(y => y.Team1.Id))
            .ForPath(x => x.Athlete1.Name, opt => opt.MapFrom(y => y.Team1.Name))
            .ForPath(x => x.Athlete1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
            .ForPath(x => x.Athlete1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
            .ForPath(x => x.Athlete1.Points, opt => opt.MapFrom(y => y.Team1.Points))
            .ForPath(x => x.Athlete1.Seed, opt => opt.MapFrom(y => y.Team1.Seed))
            .ForPath(x => x.Athlete1.Code, opt => opt.MapFrom(y => y.Team1.Code))
            .ForPath(x => x.Athlete2.Id, opt => opt.MapFrom(y => y.Team2.Id))
            .ForPath(x => x.Athlete2.Name, opt => opt.MapFrom(y => y.Team2.Name))
            .ForPath(x => x.Athlete2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
            .ForPath(x => x.Athlete2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
            .ForPath(x => x.Athlete2.Points, opt => opt.MapFrom(y => y.Team2.Points))
            .ForPath(x => x.Athlete2.Seed, opt => opt.MapFrom(y => y.Team2.Seed))
            .ForPath(x => x.Athlete2.Code, opt => opt.MapFrom(y => y.Team2.Code))
            .ReverseMap();

        this.CreateMap<MatchModel, TeamMatch<Fencing>>()
           .ForPath(x => x.Team1.Id, opt => opt.MapFrom(y => y.Team1.Id))
           .ForPath(x => x.Team1.Name, opt => opt.MapFrom(y => y.Team1.Name))
           .ForPath(x => x.Team1.NOC, opt => opt.MapFrom(y => y.Team1.NOC))
           .ForPath(x => x.Team1.MatchResult, opt => opt.MapFrom(y => y.Team1.MatchResult))
           .ForPath(x => x.Team1.Points, opt => opt.MapFrom(y => y.Team1.Points))
           .ForPath(x => x.Team1.Seed, opt => opt.MapFrom(y => y.Team1.Seed))
           .ForPath(x => x.Team2.Id, opt => opt.MapFrom(y => y.Team2.Id))
           .ForPath(x => x.Team2.Name, opt => opt.MapFrom(y => y.Team2.Name))
           .ForPath(x => x.Team2.NOC, opt => opt.MapFrom(y => y.Team2.NOC))
           .ForPath(x => x.Team2.MatchResult, opt => opt.MapFrom(y => y.Team2.MatchResult))
           .ForPath(x => x.Team2.Points, opt => opt.MapFrom(y => y.Team2.Points))
           .ForPath(x => x.Team2.Seed, opt => opt.MapFrom(y => y.Team2.Seed))
           .ReverseMap();
    }
}