using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Expense
{
    public int IdExpense { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string ExpenseName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public int? TeamId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Team? Team { get; set; }

    public virtual User User { get; set; } = null!;
}
