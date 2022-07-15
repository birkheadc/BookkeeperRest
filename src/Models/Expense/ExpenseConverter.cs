namespace BookkeeperRest.New.Models;

public class ExpenseConverter
{
    public Expense ToEntity(ExpenseDtoIncoming dto)
    {
        Expense expense = new()
        {
            Id = Guid.NewGuid(),
            Category = dto.Category,
            Amount = dto.Amount,
            Date = dto.Date,
            Note = dto.Note,
            WasTakenFromCash = dto.WasTakenFromCash
        };
        return expense;
    }

    public List<Expense> ToEntity(IEnumerable<ExpenseDtoIncoming> dtos)
    {
        List<Expense> expenses = new();

        foreach (ExpenseDtoIncoming dto in dtos)
        {
            Expense expense = ToEntity(dto);
            expenses.Add(expense);
        }

        return expenses;
    }
}