using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("users")]
[Index("Email", Name = "email_UNIQUE", IsUnique = true)]
[Index("PhoneNumber", Name = "phone_number_UNIQUE", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id_user")]
    public int IdUser { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(45)]
    public string Password { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    [InverseProperty("Owner")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    [InverseProperty("InvitedUser")]
    public virtual ICollection<Invitation> InvitationInvitedUsers { get; set; } = new List<Invitation>();

    [InverseProperty("OwnerUser")]
    public virtual ICollection<Invitation> InvitationOwnerUsers { get; set; } = new List<Invitation>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Group> GroupsNavigation { get; set; } = new List<Group>();
}
