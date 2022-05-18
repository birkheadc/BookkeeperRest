using MimeKit;

namespace BookkeeperRest.Email;

public class EmailMessage
{
    private MailboxAddress to;
    public MailboxAddress To
    {
        get
        {
            return to;
        }
    }
    private string subject;
    public string Subject
    {
        get
        {
            return subject;
        }
    }
    private string content;
    public string Content
    {
        get
        {
            return content;
        }
    }

    private List<SimpleTextAttachment> attachments = new List<SimpleTextAttachment>();
    public List<SimpleTextAttachment> Attachments
    {
        get
        {
            return attachments;
        }
    }

    public EmailMessage(string name, string address, string subject, string content)
    {
        this.to = new MailboxAddress(name, address);
        this.subject = subject;
        this.content = content;
    }

    public EmailMessage(string name, string address, string subject, string content, SimpleTextAttachment attachment)
    {
        this.to = new MailboxAddress(name, address);
        this.subject = subject;
        this.content = content;
        this.attachments.Add(attachment);
    }

    public EmailMessage(string name, string address, string subject, string content, IEnumerable<SimpleTextAttachment> attachments)
    {
        this.to = new MailboxAddress(name, address);
        this.subject = subject;
        this.content = content;
        this.attachments.AddRange(attachments);
    }
}