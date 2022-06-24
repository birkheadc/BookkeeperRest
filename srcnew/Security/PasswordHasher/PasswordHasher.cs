namespace BookkeeperRest.New.Security.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string GenerateHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool ValidateHash(string password, string hash)
    {
        bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);
        return isValid;
    }
}