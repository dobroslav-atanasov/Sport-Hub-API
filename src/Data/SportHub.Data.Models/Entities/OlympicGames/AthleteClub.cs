namespace SportHub.Data.Models.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Interfaces;

[Table("AthletesClubs", Schema = "dbo")]
public class AthleteClub : ICreatableEntity, IDeletableEntity
{
    public Guid? AthleteId { get; set; }
    public virtual Athlete Athlete { get; set; }

    public int? ClubId { get; set; }
    public virtual Club Club { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}