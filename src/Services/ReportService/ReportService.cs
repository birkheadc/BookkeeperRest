using BookkeeperRest.New.Models;
using BookkeeperRest.New.Repositories;

namespace BookkeeperRest.New.Services;

public class ReportService : IReportService
{

    private readonly IUserSettingRepository userSettingRepository;
    private readonly IEarningCategoryRepository earningCategoryRepository;
    private readonly IExpenseCategoryRepository expenseCategoryRepository;
    private readonly IDenominationRepository denominationRepository;
    private readonly IEarningRepository earningRepository;
    private readonly IExpenseRepository expenseRepository;
    private readonly EarningConverter earningConverter;
    private readonly ExpenseConverter expenseConverter;

    public ReportService(IExpenseRepository expenseRepository, IEarningRepository earningRepository, IDenominationRepository denominationRepository, IExpenseCategoryRepository expenseCategoryRepository, IEarningCategoryRepository earningCategoryRepository, IUserSettingRepository userSettingRepository)
    {
        this.expenseRepository = expenseRepository;
        this.earningRepository = earningRepository;
        this.denominationRepository = denominationRepository;
        this.expenseCategoryRepository = expenseCategoryRepository;
        this.earningCategoryRepository = earningCategoryRepository;
        this.userSettingRepository = userSettingRepository;

        this.earningConverter = new();
        this.expenseConverter = new();
    }

    public void ProcessReport(ReportDtoIncoming report)
    {
        earningRepository.RemoveEarningsByDate(report.Date);
        expenseRepository.RemoveExpensesByDate(report.Date);

        IEnumerable<Earning> earnings = earningConverter.ToEntity(report.Earnings);
        IEnumerable<Expense> expenses = expenseConverter.ToEntity(report.Expenses);

        earningRepository.AddEarnings(earnings);
        expenseRepository.AddExpenses(expenses);

        earningCategoryRepository.AddCategoriesByEarnings(earnings);
        expenseCategoryRepository.AddCategoriesByExpenses(expenses);
    }

    public ReportsWrapper GenerateReportForDates(IEnumerable<DateTime> dates)
    {
        List<Report> reports = new();

        foreach (DateTime date in dates)
        {
            Report report = GenerateReportForDate(date);
            reports.Add(report);
        }

        List<Category> earningCategories = new();
        earningCategories.AddRange(earningCategoryRepository.GetAllCategories());

        List<Category> expenseCategories = new();
        expenseCategories.AddRange(expenseCategoryRepository.GetAllCategories());

        List<Denomination> denominations = new();
        denominations.AddRange(denominationRepository.GetAllDenominations());

        string isCashDefault;
        try
        {
            isCashDefault = userSettingRepository.GetValueByName("isCashDefault");
        }
        catch (KeyNotFoundException)
        {
            isCashDefault = "true";
        }

        Summary summary = GenerateSummaryForReports(reports);

        ReportsWrapper wrapper = new()
        {
            Reports = reports,
            EarningCategories = earningCategories,
            ExpenseCategories = expenseCategories,
            Denominations = denominations,
            Summary = summary
        };

        return wrapper;
    }

    private Summary GenerateSummaryForReports(IEnumerable<Report> reports)
    {
        int numDays = 0;
        long gross = 0;
        long net = 0;
        double aveGross = 0;
        double aveNet = 0;

        Dictionary<string, long> breakdownTotals = new();

        foreach (Report report in reports)
        {
            if (report.Earnings.Count() > 0 || report.Expenses.Count() > 0)
            {
                numDays++;
                foreach (Earning earning in report.Earnings)
                {
                    gross += earning.Amount;
                    net += earning.Amount;

                    if (breakdownTotals.ContainsKey(earning.Category) == true)
                    {
                        breakdownTotals[earning.Category] += earning.Amount;
                    }
                    else
                    {
                        breakdownTotals.Add(earning.Category, earning.Amount);
                    }

                }
                foreach (Expense expense in report.Expenses)
                {
                    if (expense.WasTakenFromCash == true)
                    {
                        gross += expense.Amount;
                    }
                    else
                    {
                        net -= expense.Amount;
                    }

                    if (breakdownTotals.ContainsKey(expense.Category) == true)
                    {
                        breakdownTotals[expense.Category] -= expense.Amount;
                    }
                    else
                    {
                        breakdownTotals.Add(expense.Category, expense.Amount * -1);
                    }
                }
            }
        }
        aveGross = (double)gross / numDays;
        aveNet = (double)net / numDays;

        IEnumerable<Breakdown> breakdowns = GenerateBreakdownsFromBreakdownTotals(breakdownTotals, numDays);

        Summary summary = new()
        {
            Gross = gross,
            Net = net,
            AveGross = aveGross,
            AveNet = aveNet,
            Breakdowns = breakdowns
        };

        return summary;
    }

    private IEnumerable<Breakdown> GenerateBreakdownsFromBreakdownTotals(Dictionary<string, long> totals, int numDays)
    {
        List<Breakdown> breakdowns = new();

        foreach (KeyValuePair<string, long> total in totals)
        {
            Breakdown breakdown = new()
            {
                Category = total.Key,
                Total = total.Value,
                Average = (double)total.Value / numDays
            };

            breakdowns.Add(breakdown);
        }

        return breakdowns;
    }

    private Report GenerateReportForDate(DateTime date)
    {
        List<Earning> earnings = new();
        earnings.AddRange(earningRepository.GetEarningsByDate(date));

        List<Expense> expenses = new();
        expenses.AddRange(expenseRepository.GetExpensesByDate(date));

        Report report = new()
        {
            Date = date,
            Earnings = earnings,
            Expenses = expenses
        };

        return report;
    }

    public ReportsWrapper GenerateReportForDatesBetween(DateTime startDate, DateTime endDate)
    {
        IEnumerable<DateTime> dates = GetDatesBetween(startDate, endDate);
        return GenerateReportForDates(dates);
    }

    // TODO: This should probably be in some kid of Utility class.
    private IEnumerable<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
    {
        List<DateTime> dates = new();
        
        DateTime date = startDate;

        do
        {
            dates.Add(date);
            date = date.AddDays(1);
        }
        while (date <= endDate);

        return dates;
    }
}