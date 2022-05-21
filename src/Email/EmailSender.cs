using System;
using System.Net;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

namespace BookkeeperRest.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailConfig emailConfig;

    public EmailSender(EmailConfig emailConfig)
    {
        this.emailConfig = emailConfig;
    }

    public void SendEmail(EmailMessage message)
    {
        MimeMessage mimeMessage = CreateMimeMessage(message);
        Send(mimeMessage);
    }

    private MimeMessage CreateMimeMessage(EmailMessage message) {
        MimeMessage mimeMessage = new();
        mimeMessage.From.Add(new MailboxAddress(emailConfig.Name, emailConfig.Address));
        mimeMessage.To.Add(message.To);
        mimeMessage.Subject = message.Subject;
        
        BodyBuilder bodyBuilder = new()
        {
            TextBody = message.Content
        };

        if (message.Attachments is not null && message.Attachments.Count > 0)
        {
            byte[] fileBytes;
            foreach (SimpleTextAttachment attachment in message.Attachments)
            {
                using (MemoryStream stream = new(Encoding.UTF8.GetBytes(attachment.Content)))
                {
                    fileBytes = stream.ToArray();
                }

                bodyBuilder.Attachments.Add(attachment.FileName.Replace('/', '_').Replace(' ', '_'), fileBytes);
            }
        }

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        return mimeMessage;
    }

    private void Send(MimeMessage message)
    {
        using (SmtpClient client = new())
        {
            try
            {
                Console.WriteLine("Attempting to connect to email...");
                client.Connect(emailConfig.SmtpServer, emailConfig.Port);
                Console.WriteLine("Autenticating...");
                client.Authenticate(emailConfig.UserName, emailConfig.Password);
                Console.WriteLine("Sending...");
                client.Send(message);
            }
            catch
            {
                Console.WriteLine("Failed to send email.");
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        MimeMessage mimeMessage = CreateMimeMessage(message);
        await SendAsync(mimeMessage);
    }

    private async Task SendAsync(MimeMessage message)
    {
        using (SmtpClient client = new())
        {
            try
            {
                Console.WriteLine("Attempting to connect to email...");
                await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port);
                Console.WriteLine("Autenticating...");
                await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
                Console.WriteLine("Sending...");
                await client.SendAsync(message);
                Console.WriteLine("Sent successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to send email.");
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}