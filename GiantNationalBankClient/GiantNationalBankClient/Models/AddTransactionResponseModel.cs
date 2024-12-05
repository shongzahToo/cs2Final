namespace GiantNationalBankClient.Models
{
    public class AddTransactionResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Account Account { get; set; }
    }
}
