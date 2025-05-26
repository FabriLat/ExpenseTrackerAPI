using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

[Table("invitations")]
[Index("TeamId", Name = "fk_invitations_teams")]
[Index("InvitedUserId", Name = "id_invited_idx")]
[Index("OwnerUserId", Name = "owner_user_id_idx")]
public partial class Invitation
{
    [Key]
    [Column("id_invitation")]
    public int IdInvitation { get; set; }

    [Column("owner_user_id")]
    public int OwnerUserId { get; set; }

    [Column("invited_user_id")]
    public int InvitedUserId { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("Invitations")]
    public virtual Team Team { get; set; } = null!;

    [ForeignKey("InvitedUserId")]
    [InverseProperty("InvitationInvitedUsers")]
    public virtual User InvitedUser { get; set; } = null!;

    [ForeignKey("OwnerUserId")]
    [InverseProperty("InvitationOwnerUsers")]
    public virtual User OwnerUser { get; set; } = null!;
}
