namespace SportHub.Data.Models.Jsons.OlympicGames;

public class NOC
{
    public int Id { get; set; }

    public string Code { get; set; }

    public bool IsHistoric { get; set; }

    public bool IsMedal { get; set; }

    public Membership Membership { get; set; }

    public Interest Interest { get; set; }

    public OpenCeremony OpenCeremony { get; set; }

    public Participation Participation { get; set; }

    //public Lis Oficials { get; set; }
}

public class Participation
{
    public int? FirstAppearence { get; set; }

    public int? Appearance { get; set; }

    public string Summary { get; set; }
}

public class OpenCeremony
{
    public List<string> FlagBearers { get; set; } = [];
}

public class Interest
{
    public string Highlights { get; set; }

    public string Information { get; set; }
}

public class Membership
{
    public string Name { get; set; }

    public string OfficialName { get; set; }

    public string OfficialCommitteeName { get; set; }

    public string SEOName { get; set; }

    public int? FoundingDate { get; set; }

    public int? IOCRecognitionDate { get; set; }

    public int? DisbandedDate { get; set; }

    public string RelatedNOC { get; set; }
}