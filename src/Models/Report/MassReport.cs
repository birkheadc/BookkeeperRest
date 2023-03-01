namespace BookkeeperRest.New.Models;

public record MassReport
{
  public IEnumerable<ExpenseDtoIncoming> Expenses { get; set; }
  public IEnumerable<EarningDtoIncoming> Earnings { get; set; }
}