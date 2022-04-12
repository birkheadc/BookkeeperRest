namespace BookkeeperRest.Models;

public record Setting
{
    public string Key { get; init; } = "";
    public string Value { get; init; } = "";
}