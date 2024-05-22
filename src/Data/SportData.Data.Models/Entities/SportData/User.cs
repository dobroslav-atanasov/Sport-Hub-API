namespace SportData.Data.Models.Entities.SportData;

using System;

using global::SportData.Data.Common.Interfaces;

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