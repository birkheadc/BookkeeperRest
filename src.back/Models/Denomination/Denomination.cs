namespace BookkeeperRest.Models;

public record Denomination
{
    public int Value { get; init; }

    public bool IsDefault { get; init; }
}