namespace SportHub.Data.Models.Converters.OlympicGames.Disciplines;
using SportHub.Data.Models.Converters.OlympicGames.Base;

public class ArtisticGymnastics : BaseModel
{
    public double? Points { get; set; }

    public double? CompulsoryPoints { get; set; }

    public double? OptionalPoints { get; set; }

    public double? HalfTeamPoints { get; set; }

    public double? Time { get; set; }

    public double? Vault1 { get; set; }

    public double? Vault2 { get; set; }

    public double? VaultOff1 { get; set; }

    public double? VaultOff2 { get; set; }

    public double? Height { get; set; }

    public double? ApparatusPoints { get; set; }

    public double? Yards100Points { get; set; }

    public double? LongJumpPoints { get; set; }

    public double? ShotPutPoints { get; set; }

    public double? IndividualPoints { get; set; }

    public double? DrillPoints { get; set; }

    public double? PrecisionPoins { get; set; }

    public double? GroupExercisePoints { get; set; }

    public double? FirstTimeTrial { get; set; }

    public double? SecondTimeTrial { get; set; }

    public double? ThirdTimeTrial { get; set; }

    public List<ArtisticGymnasticsScore> Scores { get; set; } = [];

    public List<ArtisticGymnastics> Athletes { get; set; } = [];
}

public class ArtisticGymnasticsScore
{
    public string Name { get; set; }

    public double? Points { get; set; }

    public double? CompulsoryPoints { get; set; }

    public double? OptionalPoints { get; set; }

    public double? QualificationHalfPoints { get; set; }

    public double? FinalPoints { get; set; }

    public double? DScore { get; set; }

    public double? EScore { get; set; }

    public double? Penalty { get; set; }

    public double? LinePenalty { get; set; }

    public double? TimePenalty { get; set; }

    public double? OtherPenalty { get; set; }

    public double? Time { get; set; }

    public double? Vault1 { get; set; }

    public double? Vault2 { get; set; }

    public double? QualificationPoints { get; set; }

    public double? Distance { get; set; }

    public double? HalfTeamPoints { get; set; }

    public double? ApparatusPoints { get; set; }

    public double? DrillPoints { get; set; }

    public double? GroupExercisePoints { get; set; }

    public double? Yards100Points { get; set; }

    public double? LongJumpPoints { get; set; }

    public double? ShotPutPoints { get; set; }

    public double? PrecisionPoins { get; set; }
}