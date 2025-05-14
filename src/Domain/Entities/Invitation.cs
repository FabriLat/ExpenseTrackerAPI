using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

[Table("invitations")]
[Index("GroupId", Name = "fk_invitations_groups")]
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

    [Column("group_id")]
    public int GroupId { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Invitations")]
    public virtual Group Group { get; set; } = null!;

    [ForeignKey("InvitedUserId")]
    [InverseProperty("InvitationInvitedUsers")]
    public virtual User InvitedUser { get; set; } = null!;

    [ForeignKey("OwnerUserId")]
    [InverseProperty("InvitationOwnerUsers")]
    public virtual User OwnerUser { get; set; } = null!;
}
