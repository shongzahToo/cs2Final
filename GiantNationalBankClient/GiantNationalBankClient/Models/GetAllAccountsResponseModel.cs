namespace GiantNationalBankClient.Models
{
    public class GetAllAccountsResponseModel
    {
        public bool status {  get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public List<Account> accountList { get; set; }
    }
}
