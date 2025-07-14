using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Invitation
{
    public int IdInvitation { get; set; }

    public int OwnerUserId { get; set; }

    public int InvitedUserId { get; set; }

    public int TeamId { get; set; }

    public int State { get; set; }

    public virtual User InvitedUser { get; set; } = null!;

    public virtual User OwnerUser { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
