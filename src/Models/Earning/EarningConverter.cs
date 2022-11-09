namespace BookkeeperRest.New.Models;

public class EarningConverter
{
    public Earning ToEntity(EarningDtoIncoming dto)
    {
        Earning earning = new()
        {
            Id = Guid.NewGuid(),
            Category = dto.Category,
            Amount = dto.Amount,
            Date = dto.Date
        };
        return earning;
    }

    public List<Earning> ToEntity(IEnumerable<EarningDtoIncoming> dtos)
    {
        List<Earning> earnings = new();

        foreach (EarningDtoIncoming dto in dtos)
        {
            Earning earning = ToEntity(dto);
            earnings.Add(earning);
        }

        return earnings;
    }
}