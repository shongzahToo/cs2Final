namespace GiantNationalBankClient.Models
{
    public class GetAccountByIdResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public GetAccountByIdDataModel Account { get; set; }
    }
}
