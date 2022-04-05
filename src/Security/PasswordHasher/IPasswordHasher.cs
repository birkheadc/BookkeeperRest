namespace BookkeeperRest.Security.PasswordHasher;

public interface IPasswordHasher
{
    public string GenerateHash(string password);
    public bool ValidateHash(string password, string hash);
}