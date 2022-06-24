namespace BookkeeperRest.New.Services;

public interface IPasswordService
{
    public bool DoesPasswordMatch(string password);
}