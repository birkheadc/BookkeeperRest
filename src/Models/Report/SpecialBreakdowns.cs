namespace BookkeeperRest.New.Models;

public record SpecialBreakdowns
{
  public IEnumerable<MonthBreakdown> MonthBreakdowns { get; set; }
  public IEnumerable<DayOfMonthBreakdown> DayOfMonthBreakdowns { get; set; }
  public IEnumerable<DayOfWeekBreakdown> DayOfWeekBreakdowns { get; set; }

}