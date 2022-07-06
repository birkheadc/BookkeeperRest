namespace BookkeeperRest.New.Models;

public record UserSettings
{
    public UserSetting EmailName { get; set; }
    public UserSetting EmailAddress { get; set; }
}