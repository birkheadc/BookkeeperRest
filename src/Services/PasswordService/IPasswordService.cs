namespace BookkeeperRest.New.Services;

public interface IPasswordService
{
    public bool DoesPasswordMatch(string password);
    public void ChangePassword(string password);
}