namespace BookkeeperRest.New.Models.Csv;

public record CsvDto
{
    public IFormFile? File { get; init; }
}