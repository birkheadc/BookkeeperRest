namespace BookkeeperRest.Services.PasswordService;

public interface IPasswordService
{
    public bool DoesPasswordMatch(string password);
}