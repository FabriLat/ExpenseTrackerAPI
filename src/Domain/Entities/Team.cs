using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("teams")]
[Index("OwnerId", Name = "owner_id_idx")]
public partial class Team
{
    [Key]
    [Column("id_team")]
    public int IdTeam { get; set; }

    [Column("team_name")]
    [StringLength(45)]
    public string TeamName { get; set; } = null!;

    [Column("created_date")]
    public DateOnly CreatedDate { get; set; }

    [Column("owner_id")]
    public int OwnerId { get; set; }

    [InverseProperty("Team")]
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    [InverseProperty("Team")]
    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    [ForeignKey("OwnerId")]
    [InverseProperty("Teams")]
    public virtual User Owner { get; set; } = null!;

    [ForeignKey("TeamId")]
    [InverseProperty("TeamsNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
