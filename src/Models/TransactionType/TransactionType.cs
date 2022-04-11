namespace BookkeeperRest.Models;

public record TransactionType
{
    public string Name { get; init; } = "";
    public int Polarity { get; init; }  
    public bool IsDefault { get; init; }
}