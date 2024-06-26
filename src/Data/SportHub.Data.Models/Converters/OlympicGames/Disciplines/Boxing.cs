namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Boxing : BaseModel
{
    public int? Points { get; set; }

    public string Trunks { get; set; }

    public int? TotalPoints { get; set; }

    public int? InRound { get; set; }

    public string Time { get; set; }

    public BoxingDecision Decision { get; set; } = BoxingDecision.None;

    public int? Round1 { get; set; }

    public int? Round2 { get; set; }

    public int? Round3 { get; set; }

    public int? Round4 { get; set; }

    public int? Judge1 { get; set; }

    public int? Judge2 { get; set; }

    public int? Judge3 { get; set; }

    public int? Judge4 { get; set; }

    public int? Judge5 { get; set; }
}

public enum BoxingDecision
{
    None,
    Decision,
    Disqualification,
    Knockout,
    NoContest,
    Walkover,
    Retirement,
    RefereeStopsContest,
    RefereeStopsContestHeadBlow,
    RefereeStopsContestInjured,
    RefereeStopsContestOutclassed,
    RefereeStopsContestOutscored,
}