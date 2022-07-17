using BookkeeperRest.New.Models;

namespace BookkeeperRest.New.Repositories;

public interface IEarningRepository
{
    public void AddEarnings(IEnumerable<Earning> earnings);
    public void RemoveEarningsByDate(DateTime date);
    public IEnumerable<Earning> GetEarningsByDate(DateTime date);
    public IEnumerable<Earning> GetAll();
    public void RemoveAll();
}