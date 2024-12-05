namespace GiantNationalBankClient.Models
{
    public class AddTransactionModel
    {
        public int AccountID { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
    }
}
