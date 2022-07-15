using BookkeeperRest.New.Repositories;
using BookkeeperRest.New.Security.PasswordHasher;

namespace BookkeeperRest.New.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordRepository passwordRepository;
    private readonly IPasswordHasher passwordHasher;

    public PasswordService(IPasswordRepository passwordRepository)
    {
        this.passwordRepository = passwordRepository;
        passwordHasher = new PasswordHasher();
    }

    public void ChangePassword(string password)
    {
        if (IsPasswordValid(password) == false)
        {
            throw new ArgumentException();
        }
        string hash = passwordHasher.GenerateHash(password);
        passwordRepository.ChangePassword(hash);
    }

    public bool DoesPasswordMatch(string password)
    {
        string hash = passwordRepository.GetPassword();
        return passwordHasher.ValidateHash(password, hash);
    }

    private bool IsPasswordValid(string password)
    {
        // Todo: password validation, is it long enough, uses valid characters etc.
        // Abstract this out somehow
        if (password.Length < 8)
        {
            return false;
        }
        return true;
    }
}