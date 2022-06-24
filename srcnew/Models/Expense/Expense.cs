namespace BookkeeperRest.New.Models;

public record Expense
{
    public Guid Id { get; init; }
    public Category Category { get; init; }
    public long Amount { get; init; }
    public DateTime Date { get; init; }
    public string Note { get; init; }
}