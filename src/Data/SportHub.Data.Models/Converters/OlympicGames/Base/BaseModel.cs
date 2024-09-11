namespace SportHub.Data.Models.Converters.OlympicGames.Base;

using SportHub.Data.Entities.Enumerations.OlympicGames;

public abstract class BaseModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Code { get; set; }

    public string NOC { get; set; }

    public int Seed { get; set; }

    public int? Number { get; set; }

    public int? Order { get; set; }

    public FinishStatusEnum FinishStatus { get; set; }

    public bool IsQualified { get; set; }

    public int GroupNumber { get; set; }

    public RecordEnum Record { get; set; }

    //public MatchResultType MatchResult { get; set; }
}