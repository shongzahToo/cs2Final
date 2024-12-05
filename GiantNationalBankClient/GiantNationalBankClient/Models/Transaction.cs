namespace GiantNationalBankClient.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionName { get; set; }
        public bool TransactionType { get; set; }
        public double Amount { get; set; }
    }
}
