namespace SportHub.Data.Models.Converters.OlympicGames.Base;

public class Track
{
    public Guid PersonId { get; set; }

    public string PersonName { get; set; }

    public double? Length { get; set; }

    /// <summary>
    /// Gates   Curves  Total
    /// </summary>
    public int? Turns { get; set; }

    public double? StartAltitude { get; set; }

    public double? HeightDifference { get; set; }

    public int? Downstream { get; set; }

    public int? Upstream { get; set; }

    public double? MaximumClimb { get; set; }

    public double? TotalClimb { get; set; }

    public List<Intermediate> Intermediates { get; set; } = [];

    public List<ShootingInfo> ShootingInfos { get; set; } = [];
}

public class Intermediate
{
    public int Number { get; set; }

    public double? Kilometers { get; set; }
}

public class ShootingInfo
{
    public int Number { get; set; }

    public string Info { get; set; }
}