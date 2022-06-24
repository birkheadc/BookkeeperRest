using BookkeeperRest.Repositories.PasswordRepository;
using BookkeeperRest.Security.PasswordHasher;

namespace BookkeeperRest.Services.PasswordService;

public class PasswordService : IPasswordService
{
    private IPasswordHasher hasher;
    private IPasswordRepository repository;

    public PasswordService(IPasswordHasher hasher, IPasswordRepository repository)
    {
        this.hasher = hasher;
        this.repository = repository;
    }

    public bool ChangePassword(string oldPassword, string newPassword)
    {
        if (DoesPasswordMatch(oldPassword) == false)
        {
            return false;
        }
        repository.Change(hasher.GenerateHash(newPassword));
        return true;
    }

    public bool DoesPasswordMatch(string password)
    {
        string hash = repository.Get();

        // If hash is empty, it means no password has been set. Allow all operations in that case.
        if (hash == "")
        {
            return true;
        }

        return hasher.ValidateHash(password, hash);
    }
}