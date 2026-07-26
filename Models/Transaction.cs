namespace Auditor.Models;

public enum TransactionType
{
    Income = 1,
    Expense = 2
}

public class Transaction
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }

    public bool IsFlagged { get; set; } = false;
}