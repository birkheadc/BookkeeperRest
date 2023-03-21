namespace BookkeeperRest.New.Models;

public record DayOfWeekBreakdown
{
  public int DayOfWeek { get; set; }
  public long AverageAmount { get; set; }
}