namespace BookkeeperRest.New.Repositories;

public interface IPasswordRepository
{
    public string GetPassword();
    public void ChangePassword(string password);
}