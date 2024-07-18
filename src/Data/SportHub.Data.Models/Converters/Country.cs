namespace SportHub.Data.Models.Converters;

public class Country
{
    public string Name { get; set; }

    public string OfficialName { get; set; }

    public bool IsIndependent { get; set; } = false;

    public string TwoDigitsCode { get; set; }

    public string Code { get; set; }

    public string Capital { get; set; }

    public string Continent { get; set; }

    public string MemberOf { get; set; }

    public int Population { get; set; }

    public int TotalArea { get; set; }

    public string HighestPointPlace { get; set; }

    public int? HighestPoint { get; set; }

    public string LowestPointPlace { get; set; }

    public int? LowestPoint { get; set; }

    public byte[] Flag { get; set; }
}