using System;
using System.Collections.Generic;

namespace FaixariT.HRMS.DBModels;

public partial class Users
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime? DateJoined { get; set; }

    public bool? IsActive { get; set; }
}
