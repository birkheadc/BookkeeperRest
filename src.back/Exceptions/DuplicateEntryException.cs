public class DuplicateEntryException : Exception
{
    public DuplicateEntryException()
    {

    }
    public DuplicateEntryException(string message)
        : base(message)
    {

    }
    public DuplicateEntryException(string message, Exception inner)
        : base(message, inner)
    {

    }
}