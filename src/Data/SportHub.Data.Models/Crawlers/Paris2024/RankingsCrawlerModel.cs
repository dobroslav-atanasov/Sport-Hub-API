namespace SportHub.Data.Models.Crawlers.Paris2024.Rankings;

using System.Text.Json.Serialization;

public class RankingsCrawlerModel
{
    [JsonPropertyName("event")]
    public EventInfo Event { get; set; }
}

public class EventInfo
{
    [JsonPropertyName("rankings")]
    public List<Ranking> Rankings { get; set; }
}

public class MainFunction
{
    [JsonPropertyName("category")]
    public string Category { get; set; }
}

public class Organisation
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class ParticipantInfo
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("current")]
    public bool Current { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("givenName")]
    public string GivenName { get; set; }

    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; }

    [JsonPropertyName("mainFunction")]
    public MainFunction MainFunction { get; set; }

    [JsonPropertyName("organisation")]
    public Organisation Organisation { get; set; }

    [JsonPropertyName("__typename")]
    public string Typename { get; set; }

    [JsonPropertyName("personGender")]
    public PersonGender PersonGender { get; set; }

    [JsonPropertyName("teamType")]
    public string TeamType { get; set; }


}

public class PersonGender
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Ranking
{
    [JsonPropertyName("event_code")]
    public string EventCode { get; set; }

    [JsonPropertyName("rk_Played")]
    public string RkPlayed { get; set; }

    [JsonPropertyName("rk_Won")]
    public string RkWon { get; set; }

    [JsonPropertyName("rk_Lost")]
    public string RkLost { get; set; }

    [JsonPropertyName("rk_IRM")]
    public string RkIRM { get; set; }

    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; set; }

    [JsonPropertyName("rk_Type")]
    public string RkType { get; set; }

    [JsonPropertyName("athlete_code")]
    public string AthleteCode { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }

    [JsonPropertyName("rank")]
    public string Rank { get; set; }

    [JsonPropertyName("rk_RankEqual")]
    public string RkRankEqual { get; set; }

    [JsonPropertyName("resultType")]
    public string ResultType { get; set; }

    [JsonPropertyName("rk_Result")]
    public string RkResult { get; set; }

    [JsonPropertyName("team_code")]
    public string TeamCode { get; set; }

    [JsonPropertyName("teamAthletes")]
    public List<TeamAthlete> TeamAthletes { get; set; }

    [JsonPropertyName("rk_Tied")]
    public string RkTied { get; set; }
}

public class TeamAthlete
{
    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("participant")]
    public ParticipantInfo Participant { get; set; }
}