﻿namespace SportData.Data.Models.Converters.OlympicGames.Base;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public class Round<TModel>
{
    public string Name { get; set; }

    public RoundTypeEnum Type { get; set; }

    public RoundTypeEnum SubType { get; set; }

    public int Number { get; set; }

    public string Info { get; set; }

    public string EventName { get; set; }

    public string Format { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public Track Track { get; set; }

    public List<Judge> Judges { get; set; } = [];

    public List<TModel> Athletes { get; set; } = [];

    public List<AthleteMatch<TModel>> AthleteMatches { get; set; } = [];

    public List<TModel> Teams { get; set; } = [];

    public List<TeamMatch<TModel>> TeamMatches { get; set; } = [];
}