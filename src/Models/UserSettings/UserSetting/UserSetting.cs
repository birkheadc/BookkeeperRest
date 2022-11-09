namespace BookkeeperRest.New.Models;

public record UserSetting
{
    public string Name { get; init; }
    public string Value { get; init; }
}