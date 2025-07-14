using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Invitation> InvitationInvitedUsers { get; set; } = new List<Invitation>();

    public virtual ICollection<Invitation> InvitationOwnerUsers { get; set; } = new List<Invitation>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    public virtual ICollection<Team> TeamsNavigation { get; set; } = new List<Team>();
}
