using System.Transactions;

namespace GiantNationalBankClient.Models
{
    public class GetAccountByIdDataModel
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public double Balance { get; set; }
        public User user { get; set; }
        public List<Transaction> TransactionList { get; set; }
    }
}
