namespace SportHub.Services.Interfaces;

using SportHub.Data.Models.Converters;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Entities.OlympicGames.Enumerations;

public interface IOlympediaService
{
    List<AthleteModel> FindAthletes(string text);

    IList<string> FindNOCCodes(string text);

    List<int> FindVenues(string text);

    AthleteModel FindAthlete(string text);

    List<int> FindClubs(string text);

    string FindNOCCode(string text);

    MedalTypeEnum FindMedal(string text);

    MedalTypeEnum FindMedal(string text, RoundTypeEnum roundType);

    FinishTypeEnum FindStatus(string text);

    int FindMatchNumber(string text);

    int FindResultNumber(string text);

    string FindLocation(string html);

    string FindMatchInfo(string text);

    RecordType FindRecord(string text);

    QualificationType FindQualification(string text);

    IList<int> FindResults(string text);

    DecisionType FindDecision(string text);

    int FindSeedNumber(string text);

    void SetWinAndLose(MatchModel match);

    Horse FindHorse(string text);

    Dictionary<string, int> GetIndexes(List<string> headers);

    Dictionary<string, int> FindIndexes(List<string> headers);
}