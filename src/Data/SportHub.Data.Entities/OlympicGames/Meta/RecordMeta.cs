namespace SportHub.Data.Entities.OlympicGames.Meta;

public class RecordMeta
{
    public List<RecordDataMeta> Data { get; set; } = [];

    public CompetitorMeta Competitor { get; set; }
}

public class CompetitorMeta
{
    public string Code { get; set; }

    public string OrganisationCode { get; set; }

    public string OrganisationName { get; set; }

    public string OrganisationLongName { get; set; }

    public string Description { get; set; }

    public bool IsTeam { get; set; } = false;

    public string Name { get; set; }

    public string ShortName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public List<ParticipantMeta> Participants { get; set; } = [];
}

public class ParticipantMeta
{
    public int Order { get; set; }

    public string Code { get; set; }

    public DateTime? BirthDate { get; set; }

    public bool IsTeam { get; set; } = false;

    public string Name { get; set; }

    public string Gender { get; set; }

    public string ShortName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Organisation { get; set; }
}

public class RecordDataMeta
{
    public string Key { get; set; }

    public string Value { get; set; }
}