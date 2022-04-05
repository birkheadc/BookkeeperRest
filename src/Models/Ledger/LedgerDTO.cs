using BookkeeperRest.Models.Earning;
using BookkeeperRest.Models.Expense;

namespace BookkeeperRest.Models.Ledger;

[Serializable]
public record LedgerDTO
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public IEnumerable<EarningDTO> Earnings { get; init; }
    public IEnumerable<ExpenseDTO> Expenses { get; init; }
}