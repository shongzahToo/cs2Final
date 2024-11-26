namespace GiantNationalBankClient.Models
{
    public class GetUserByIdResponseModel
    {
        public bool status { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public User user { get; set; }
    }
}
