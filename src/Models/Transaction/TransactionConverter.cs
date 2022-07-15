namespace BookkeeperRest.New.Models;

public class TransactionConverter
{
    public Earning ToEarning(TransactionDto dto)
    {
        Earning earning = new()
        {
            Id = Guid.NewGuid(),
            Category = dto.Category,
            Amount = Math.Abs(dto.Amount),
            Date = dto.Date
        };
        return earning;
    }

    public Expense ToExpense(TransactionDto dto)
    {
        Expense expense = new()
        {
            Id = Guid.NewGuid(),
            Category = dto.Category,
            Amount = Math.Abs(dto.Amount),
            Date = dto.Date,
            Note = dto.Note,
            WasTakenFromCash = dto.WasTakenFromCash
        };
        return expense;
    }
}