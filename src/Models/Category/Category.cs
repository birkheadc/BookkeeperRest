namespace BookkeeperRest.New.Models;

public record Category
{
    public string Name { get; init; }
    public bool IsDefault { get; init; }
}