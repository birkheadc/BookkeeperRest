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

        ReportsWrapper wrapper = new()
        {
            Reports = reports,
            EarningCategories = earningCategories,
            ExpenseCategories = expenseCategories,
            Denominations = denominations,
            IsCashDefault = isCashDefault
        };

        return wrapper;
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