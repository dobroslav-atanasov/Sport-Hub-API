namespace SportHub.Data.Models.Entities.SportHub;

using System;

using global::SportHub.Data.Common.Interfaces;

using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole, ICreatableEntity
{
    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}