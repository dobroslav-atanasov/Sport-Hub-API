namespace SportData.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations.Schema;

using global::SportData.Data.Common.Interfaces;

[Table("Squads", Schema = "dbo")]
public class Squad : ICreatableEntity, IDeletableEntity
{
    public Guid? ParticipationId { get; set; }
    public virtual Participation Participation { get; set; }

    public Guid? TeamId { get; set; }
    public virtual Team Team { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}