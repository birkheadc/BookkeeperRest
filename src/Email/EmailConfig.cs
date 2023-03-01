namespace BookkeeperRest.New.Email;

public class EmailConfig
{
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string SmtpServer { get; set; } = "";
    public int Port { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public bool EmailEnabled { get; set; } = false;

}