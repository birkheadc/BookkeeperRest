namespace BookkeeperRest.Models.Transaction;

public class TransactionConverter : ITransactionConverter
{
    public TransactionDTO ToDTO(Transaction transaction)
    {
        TransactionDTO dto = new()
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Type = transaction.Type,
            Amount = transaction.Amount,
            Note = transaction.Note
        };
        return dto;
    }

    public IEnumerable<TransactionDTO> ToDTO(IEnumerable<Transaction> transactions)
    {
        List<TransactionDTO> dtos = new();
        foreach (Transaction transaction in transactions)
        {
            dtos.Add(ToDTO(transaction));
        }
        return dtos;
    }

    public Transaction ToEntity(TransactionDTO dto)
    {
        Transaction transaction = new()
        {
            Id = dto.Id,
            Date = dto.Date,
            Type = dto.Type,
            Amount = dto.Amount,
            Note = dto.Note
        };
        return transaction;
    }

    public IEnumerable<Transaction> ToEntity(IEnumerable<TransactionDTO> dtos)
    {
        List<Transaction> entities = new();
        foreach (TransactionDTO dto in dtos)
        {
            entities.Add(ToEntity(dto));
        }
        return entities;
    }

    public Transaction ToEntity(NewTransactionDTO dto)
    {
        if (IsNameValid(dto.Type) == false)
        {
            throw new ArgumentException();
        }
        Transaction transaction = new()
        {
            Id = Guid.NewGuid(),
            Date = dto.Date,
            Type = ConvertName(dto.Type),
            Amount = dto.Amount,
            Note = dto.Note
        };
        return transaction;
    }

    public IEnumerable<Transaction> ToEntity(IEnumerable<NewTransactionDTO> dtos)
    {
        List<Transaction> entities = new();
        foreach (NewTransactionDTO dto in dtos)
        {
            entities.Add(ToEntity(dto));
        }
        return entities;
    }

    public bool IsNameValid(string name) {
        foreach (char c in name) {
            if (Char.IsLetter(c)) {
                continue;
            }
            if (c != '_' && c != ' ' && c != '-') {
                return false;
            }
        }
        return true;
    }

    public string ConvertName(string name)
    {
        string newName = name.Replace(' ', '_');
        newName = newName.ToLower();
        return newName;
    }
}