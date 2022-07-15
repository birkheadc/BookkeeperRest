namespace BookkeeperRest.New.Models;

public record Denomination
{
    public int Value { get; init; }
    public bool IsDefault { get; init; }
}