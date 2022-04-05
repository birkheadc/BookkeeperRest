namespace BookkeeperRest.Repositories.PasswordRepository;

public interface IPasswordRepository
{
    public void Change(string newHashedPassword);
    public string Get();
}