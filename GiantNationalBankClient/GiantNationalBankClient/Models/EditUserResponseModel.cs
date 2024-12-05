using System;

namespace GiantNationalBankClient.Models;

public class EditUserResponseModel
{
    public bool Status {  get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public User Data { get; set; }
}
