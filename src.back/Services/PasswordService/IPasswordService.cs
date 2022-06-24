namespace BookkeeperRest.Services.PasswordService;

public interface IPasswordService
{
    public bool ChangePassword(string oldPassword, string newPassword);
    public bool DoesPasswordMatch(string password);
}