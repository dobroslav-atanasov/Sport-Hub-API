namespace SportData.Data.Models.Converters.OlympicGames;

using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public class MatchInputModel
{
    public string Row { get; set; }

    public string Number { get; set; }

    public string Date { get; set; }

    public string Location { get; set; }

    public int Year { get; set; }

    public Guid EventId { get; set; }

    public bool IsTeam { get; set; }

    public bool IsDoubles { get; set; } = false;

    public string HomeName { get; set; }

    public string HomeNOC { get; set; }

    public string AwayName { get; set; }

    public string AwayNOC { get; set; }

    public string Result { get; set; }

    public bool AnyParts { get; set; }

    public RoundTypeEnum RoundType { get; set; }

    public RoundTypeEnum RoundSubType { get; set; }
}