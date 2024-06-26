namespace SportHub.Data.Common.Models;

using System.ComponentModel.DataAnnotations;

using SportHub.Data.Common.Interfaces;

public abstract class BaseEntity<TKey> : ICreatableEntity
{
    [Key]
    public TKey Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}