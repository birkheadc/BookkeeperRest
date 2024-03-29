namespace BookkeeperRest.New.Email;

public interface IEmailSender
{
    public void SendEmail(EmailMessage mesasge);
    public Task SendEmailAsync(EmailMessage message);
}