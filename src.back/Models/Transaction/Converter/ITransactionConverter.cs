namespace BookkeeperRest.Models.Transaction;

public interface ITransactionConverter
{
    public TransactionDTO ToDTO(Transaction transaction);
    public IEnumerable<TransactionDTO> ToDTO(IEnumerable<Transaction> transactions);
    public Transaction ToEntity(TransactionDTO dto);
    public IEnumerable<Transaction> ToEntity(IEnumerable<TransactionDTO> dtos);
    public Transaction ToEntity(NewTransactionDTO dto);
    public IEnumerable<Transaction> ToEntity(IEnumerable<NewTransactionDTO> dtos);
    public bool IsNameValid(string name);
    public string ConvertName(string name);


}