using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("expenses")]
[Index("GroupId", Name = "group_Id_idx")]
[Index("UserId", Name = "user_id_idx")]
public partial class Expense
{
    [Key]
    [Column("id_expense")]
    public int IdExpense { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("expense_name")]
    [StringLength(45)]
    public string ExpenseName { get; set; } = null!;

    [Column("description")]
    [StringLength(155)]
    public string? Description { get; set; }

    [Column("expense_date")]
    public DateOnly ExpenseDate { get; set; }

    [Column("group_id")]
    public int? GroupId { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Expenses")]
    public virtual Group? Group { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Expenses")]
    public virtual User User { get; set; } = null!;
}