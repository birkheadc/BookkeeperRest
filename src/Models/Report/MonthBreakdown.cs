namespace BookkeeperRest.New.Models;

public record MonthBreakdown
{
  public int Month { get; set; }
  public long AverageAmount { get; set; }
}