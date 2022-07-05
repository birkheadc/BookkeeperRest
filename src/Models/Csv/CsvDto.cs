namespace BookkeeperRest.New.Models;

public record CsvDto
{
    public IFormFile? File { get; init; }
}