using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("groups")]
[Index("OwnerId", Name = "owner_id_idx")]
public partial class Group
{
    [Key]
    [Column("id_group")]
    public int IdGroup { get; set; }

    [Column("group_name")]
    [StringLength(45)]
    public string GroupName { get; set; } = null!;

    [Column("created_date")]
    public DateOnly CreatedDate { get; set; }

    [Column("owner_id")]
    public int OwnerId { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    [InverseProperty("Group")]
    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    [ForeignKey("OwnerId")]
    [InverseProperty("Groups")]
    public virtual User Owner { get; set; } = null!;

    [ForeignKey("GroupId")]
    [InverseProperty("GroupsNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
