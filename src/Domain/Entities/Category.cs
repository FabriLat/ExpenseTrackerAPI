using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Category
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
