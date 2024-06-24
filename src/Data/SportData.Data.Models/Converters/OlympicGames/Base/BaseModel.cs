namespace SportData.Data.Models.Converters.OlympicGames.Base;

using SportData.Data.Models.Entities.Enumerations;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public abstract class BaseModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Code { get; set; }

    public string NOC { get; set; }

    public int Seed { get; set; }

    public int? Number { get; set; }

    public int? Order { get; set; }

    public FinishTypeEnum FinishStatus { get; set; }

    public QualificationType Qualification { get; set; }

    public int GroupNumber { get; set; }

    public RecordType Record { get; set; }

    public MatchResultType MatchResult { get; set; }
}