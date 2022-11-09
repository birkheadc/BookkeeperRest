using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Repositories;

public interface IExpenseRepository
{
    public void AddExpenses(IEnumerable<Expense> expenses);
    public void RemoveExpensesByDate(DateTime date);
    public IEnumerable<Expense> GetExpensesByDate(DateTime date);
    public IEnumerable<Expense> GetAll();
    public void RemoveAll();
}