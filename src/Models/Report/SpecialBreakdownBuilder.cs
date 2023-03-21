namespace BookkeeperRest.New.Models;

public class SpecialBreakdownBuilder
{
  private Dictionary<DateTime, List<Earning>> _earnings = new();
  private Dictionary<int, List<long>> monthAmounts = new();
  private Dictionary<int, List<long>> dayOfMonthAmounts = new();
  private Dictionary<int, List<long>> dayOfWeekAmounts = new();

  public void AddEarnings(IEnumerable<Earning> earnings)
  {
    foreach (Earning earning in earnings)
    {
      if (_earnings.ContainsKey(earning.Date.Date) == false)
      {
        _earnings.Add(earning.Date.Date, new List<Earning>());
      }

      _earnings[earning.Date.Date].Add(earning);
    }
  }

  public void AddExpenses(IEnumerable<Expense> expenses)
  {
    foreach (Expense expense in expenses)
    {
      if (expense.WasTakenFromCash == true)
      {
        if (_earnings.ContainsKey(expense.Date.Date) == false)
        {
          _earnings.Add(expense.Date.Date, new List<Earning>());
        }

        _earnings[expense.Date.Date].Add(new Earning()
        {
          Id = Guid.NewGuid(),
          Category = "Cash Expense",
          Amount = expense.Amount,
          Date = expense.Date
        });
      }
    }
  }

  public SpecialBreakdowns Build()
  {
    FinalizeAmounts();

    List<MonthBreakdown> monthBreakdowns = new();
    List<DayOfMonthBreakdown> dayOfMonthBreakdowns = new();
    List<DayOfWeekBreakdown> dayOfWeekBreakdowns = new();

    foreach(KeyValuePair<int, List<long>> monthAmount in monthAmounts)
    {
      monthBreakdowns.Add(new MonthBreakdown()
      {
        Month = monthAmount.Key,
        AverageAmount = (long)Math.Round(monthAmount.Value.Average())
      });
    }

    foreach(KeyValuePair<int, List<long>> dayOfMonthAmount in dayOfMonthAmounts)
    {
      dayOfMonthBreakdowns.Add(new DayOfMonthBreakdown()
      {
        DayOfMonth = dayOfMonthAmount.Key,
        AverageAmount = (long)Math.Round(dayOfMonthAmount.Value.Average())
      });
    }

    foreach(KeyValuePair<int, List<long>> dayOfWeekAmount in dayOfWeekAmounts)
    {
      dayOfWeekBreakdowns.Add(new DayOfWeekBreakdown()
      {
        DayOfWeek = dayOfWeekAmount.Key,
        AverageAmount = (long)Math.Round(dayOfWeekAmount.Value.Average())
      });
    }

    return new SpecialBreakdowns()
    {
      MonthBreakdowns = monthBreakdowns,
      DayOfMonthBreakdowns = dayOfMonthBreakdowns,
      DayOfWeekBreakdowns = dayOfWeekBreakdowns
    };
  }

  private void FinalizeAmounts()
  {
    foreach (KeyValuePair<DateTime, List<Earning>> earnings in _earnings)
    {
      int month = earnings.Key.Date.Month;
      int dayOfMonth = earnings.Key.Date.Day;
      int dayOfWeek = (int)earnings.Key.Date.DayOfWeek + 1;

      if (monthAmounts.ContainsKey(month) == false) monthAmounts.Add(month, new List<long>());
      if (dayOfMonthAmounts.ContainsKey(dayOfMonth) == false) dayOfMonthAmounts.Add(dayOfMonth, new List<long>());
      if (dayOfWeekAmounts.ContainsKey(dayOfWeek) == false) dayOfWeekAmounts.Add(dayOfWeek, new List<long>());

      long amount = 0;

      foreach (Earning earning in earnings.Value)
      {
        amount += earning.Amount;
      }

      monthAmounts[month].Add(amount);
      dayOfMonthAmounts[dayOfMonth].Add(amount);
      dayOfWeekAmounts[dayOfWeek].Add(amount);
    }
  }
}