namespace GiantNationalBankClient.Models
{
    public class GetUserByIdResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
