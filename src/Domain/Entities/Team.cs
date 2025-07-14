using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Team
{
    public int IdTeam { get; set; }

    public string TeamName { get; set; } = null!;

    public DateOnly CreatedDate { get; set; }

    public int OwnerId { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
