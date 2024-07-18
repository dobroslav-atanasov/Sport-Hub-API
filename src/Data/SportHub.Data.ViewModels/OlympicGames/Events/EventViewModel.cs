namespace SportHub.Data.ViewModels.OlympicGames.Events;

public class EventViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string OriginalName { get; set; }

    public string NormalizedName { get; set; }

    public int? EventGenderTypeId { get; set; }

    public int? DisciplineId { get; set; }

    public int? GameId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsTeamEvent { get; set; }

    public string AdditionalInfo { get; set; }

    public string Format { get; set; }

    public string Description { get; set; }
}