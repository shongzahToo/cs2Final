namespace GiantNationalBankClient.Models
{
    public class GetAllAccountsResponseModel
    {
        public bool Status {  get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Account> AccountList { get; set; }
    }
}
