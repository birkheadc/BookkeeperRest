namespace BookkeeperRest.New.Models;

public record DayOfMonthBreakdown
{
  public int DayOfMonth { get; set; }
  public long AverageAmount { get; set; }
}