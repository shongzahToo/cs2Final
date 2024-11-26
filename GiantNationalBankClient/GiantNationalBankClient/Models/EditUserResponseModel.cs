using System;

namespace GiantNationalBankClient.Models;

public class EditUserResponseModel
{
    public bool status {  get; set; }
    public int statusCode { get; set; }
    public string message { get; set; }
    public User Data { get; set; }
}
