using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Services;

public interface IReportService
{
    public void ProcessReport(ReportDtoIncoming report);
    public void ProcessMassReport(MassReport report);
    public ReportsWrapper GenerateReportForDates(IEnumerable<DateTime> dates);
    public ReportsWrapper GenerateReportForDatesBetween(DateTime startDate, DateTime endDate);
    public void ProcessCsv(IFormFile file);
}