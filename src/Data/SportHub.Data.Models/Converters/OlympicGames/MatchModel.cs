namespace SportHub.Data.Models.Converters.OlympicGames;

using SportHub.Data.Models.DbEntities.Enumerations;
using SportHub.Data.Models.Enumerations.OlympicGames;

public class MatchModel
{
    public int Number { get; set; }

    public DateTime? Date { get; set; }

    public string Location { get; set; }

    public DecisionEnum Decision { get; set; }

    public int ResultId { get; set; }

    public MedalEnum Medal { get; set; }

    public string Info { get; set; }

    public MatchTeamModel Team1 { get; set; } = new();

    public MatchTeamModel Team2 { get; set; } = new();
}

public class MatchTeamModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string NOC { get; set; }

    public int Code { get; set; }

    public int Seed { get; set; }

    public MatchResultType MatchResult { get; set; } = MatchResultType.None;

    public int Points { get; set; }

    public TimeSpan? Time { get; set; }

    public List<int?> Parts { get; set; } = [];
}

public class MatchPartModel
{
    public int Number { get; set; }

    public int Points { get; set; }
}