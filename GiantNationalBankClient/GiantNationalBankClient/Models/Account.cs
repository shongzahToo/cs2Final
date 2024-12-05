namespace GiantNationalBankClient.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }
        public decimal Balance { get; set; }
        public User? User { get; set; }
        public List<Transaction> TransactionList { get; set; }
    }
}
