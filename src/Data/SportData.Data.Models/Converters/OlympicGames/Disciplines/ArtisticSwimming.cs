namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;
using SportData.Data.Models.Converters.OlympicGames.Base;

public class ArtisticSwimming : BaseModel
{
    public double? Points { get; set; }

    public double? MusicalRoutinePoints { get; set; }

    public double? FigurePoints { get; set; }

    public double? TechnicalRoutinePoints { get; set; }

    public double? FreeRoutinePoints { get; set; }

    public double? TechnicalMerit { get; set; }
    public double? ArtisticImpression { get; set; }
    public double? Execution { get; set; }
    public double? OverallImpression { get; set; }
    public double? ReducedPoints { get; set; }

    public double? ExecutionJudge1 { get; set; }
    public double? ExecutionJudge2 { get; set; }
    public double? ExecutionJudge3 { get; set; }
    public double? ExecutionJudge4 { get; set; }
    public double? ExecutionJudge5 { get; set; }
    public double? ExecutionJudge6 { get; set; }
    public double? ExecutionJudge7 { get; set; }

    public double? OverallImpressionJudge1 { get; set; }
    public double? OverallImpressionJudge2 { get; set; }
    public double? OverallImpressionJudge3 { get; set; }
    public double? OverallImpressionJudge4 { get; set; }
    public double? OverallImpressionJudge5 { get; set; }
    public double? OverallImpressionJudge6 { get; set; }
    public double? OverallImpressionJudge7 { get; set; }

    public double? Routine1Points { get; set; }
    public double? Routine2Points { get; set; }
    public double? Routine3Points { get; set; }
    public double? Routine4Points { get; set; }
    public double? Routine5Points { get; set; }

    public double? RequiredElementPenalty { get; set; }
    public double? Difficulty { get; set; }
    public double? Routine1DegreeOfDifficulty { get; set; }
    public double? Routine2DegreeOfDifficulty { get; set; }
    public double? Routine3DegreeOfDifficulty { get; set; }
    public double? Routine4DegreeOfDifficulty { get; set; }
    public double? Routine5DegreeOfDifficulty { get; set; }

    public double? TechnicalMeritExecution { get; set; }
    public double? TechnicalMeritSynchronization { get; set; }
    public double? TechnicalMeritDifficulty { get; set; }
    public double? ArtisticImpressionChoreography { get; set; }
    public double? ArtisticImpressionMusicInterpretation { get; set; }
    public double? ArtisticImpressionMannerOfPresentation { get; set; }

    public double? ArtisticImpressionJudge1 { get; set; }
    public double? ArtisticImpressionJudge2 { get; set; }
    public double? ArtisticImpressionJudge3 { get; set; }
    public double? ArtisticImpressionJudge4 { get; set; }
    public double? ArtisticImpressionJudge5 { get; set; }

    public double? DifficultyJudge1 { get; set; }
    public double? DifficultyJudge2 { get; set; }
    public double? DifficultyJudge3 { get; set; }
    public double? DifficultyJudge4 { get; set; }
    public double? DifficultyJudge5 { get; set; }
    public double? Penalties { get; set; }

    public List<ArtisticSwimming> Athletes { get; set; } = [];
}

public class TechnicalRoutine
{
    public double? Points { get; set; }
    public double? TechnicalMerit { get; set; }
    public double? ArtisticImpression { get; set; }
    public double? Execution { get; set; }
    public double? OverallImpression { get; set; }

    public double? ExecutionJudge1 { get; set; }
    public double? ExecutionJudge2 { get; set; }
    public double? ExecutionJudge3 { get; set; }
    public double? ExecutionJudge4 { get; set; }
    public double? ExecutionJudge5 { get; set; }
    public double? ExecutionJudge6 { get; set; }
    public double? ExecutionJudge7 { get; set; }

    public double? OverallImpressionJudge1 { get; set; }
    public double? OverallImpressionJudge2 { get; set; }
    public double? OverallImpressionJudge3 { get; set; }
    public double? OverallImpressionJudge4 { get; set; }
    public double? OverallImpressionJudge5 { get; set; }
    public double? OverallImpressionJudge6 { get; set; }
    public double? OverallImpressionJudge7 { get; set; }

    public double? Routine1Points { get; set; }
    public double? Routine2Points { get; set; }
    public double? Routine3Points { get; set; }
    public double? Routine4Points { get; set; }
    public double? Routine5Points { get; set; }

    public double? RequiredElementPenalty { get; set; }
    public double? Difficulty { get; set; }
    public double? Routine1DegreeOfDifficulty { get; set; }
    public double? Routine2DegreeOfDifficulty { get; set; }
    public double? Routine3DegreeOfDifficulty { get; set; }
    public double? Routine4DegreeOfDifficulty { get; set; }
    public double? Routine5DegreeOfDifficulty { get; set; }
}

public class FreeRoutine
{
    public double? Points { get; set; }
    public double? TechnicalMerit { get; set; }
    public double? ArtisticImpression { get; set; }
    public double? Execution { get; set; }
    public double? OverallImpression { get; set; }
    public double? TechnicalMeritExecution { get; set; }
    public double? TechnicalMeritSynchronization { get; set; }
    public double? TechnicalMeritDifficulty { get; set; }
    public double? ArtisticImpressionChoreography { get; set; }
    public double? ArtisticImpressionMusicInterpretation { get; set; }
    public double? ArtisticImpressionMannerOfPresentation { get; set; }

    public double? ExecutionJudge1 { get; set; }
    public double? ExecutionJudge2 { get; set; }
    public double? ExecutionJudge3 { get; set; }
    public double? ExecutionJudge4 { get; set; }
    public double? ExecutionJudge5 { get; set; }

    public double? ArtisticImpressionJudge1 { get; set; }
    public double? ArtisticImpressionJudge2 { get; set; }
    public double? ArtisticImpressionJudge3 { get; set; }
    public double? ArtisticImpressionJudge4 { get; set; }
    public double? ArtisticImpressionJudge5 { get; set; }

    public double? Difficulty { get; set; }
    public double? DifficultyJudge1 { get; set; }
    public double? DifficultyJudge2 { get; set; }
    public double? DifficultyJudge3 { get; set; }
    public double? DifficultyJudge4 { get; set; }
    public double? DifficultyJudge5 { get; set; }
    public double? Penalties { get; set; }
}