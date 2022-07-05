using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Services;

public interface IReportService
{
    public void ProcessReport(ReportDtoIncoming report);
    public ReportsWrapper GenerateReportForDates(IEnumerable<DateTime> dates);
    public ReportsWrapper GenerateReportForDatesBetween(DateTime startDate, DateTime endDate);
}