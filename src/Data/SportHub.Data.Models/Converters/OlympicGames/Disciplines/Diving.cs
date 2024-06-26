namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;

using SportHub.Data.Models.Converters.OlympicGames.Base;

public class Diving : BaseModel
{
    public double? Points { get; set; }

    public double? CompulsoryPoints { get; set; }

    public double? FinalPoints { get; set; }

    public double? SemiFinalsPoints { get; set; }

    public double? QualificationPoints { get; set; }

    public double? Ordinals { get; set; }

    public List<Dive> Dives { get; set; } = [];

    public List<Diving> Athletes { get; set; } = [];
}

public class Dive
{
    public int Number { get; set; }
    public double? Points { get; set; }
    public double? Difficulty { get; set; }
    public string Name { get; set; }
    public double? Ordinals { get; set; }

    public double? ExecutionJudge1Score { get; set; }
    public double? ExecutionJudge2Score { get; set; }
    public double? ExecutionJudge3Score { get; set; }
    public double? ExecutionJudge4Score { get; set; }
    public double? ExecutionJudge5Score { get; set; }
    public double? ExecutionJudge6Score { get; set; }
    public double? ExecutionJudge7Score { get; set; }

    public double? SynchronizationJudge1Score { get; set; }
    public double? SynchronizationJudge2Score { get; set; }
    public double? SynchronizationJudge3Score { get; set; }
    public double? SynchronizationJudge4Score { get; set; }
    public double? SynchronizationJudge5Score { get; set; }
}