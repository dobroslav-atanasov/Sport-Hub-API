namespace SportHub.Data.Common.Interfaces;

public interface ICreatableEntity
{
    DateTime CreatedOn { get; set; }

    DateTime? ModifiedOn { get; set; }
}