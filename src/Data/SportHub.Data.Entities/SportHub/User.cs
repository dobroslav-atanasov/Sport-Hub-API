namespace SportHub.Data.Entities.SportHub;

using System;

using global::SportHub.Data.Common.Interfaces;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser, ICreatableEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string PhotoUrl { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}