
namespace BookkeeperRest.New.Models;
public record Report
{
    public DateTime Date { get; init; }
    public IEnumerable<Earning> Earnings { get; init; }
    public IEnumerable<Expense> Expenses { get; init; }
}