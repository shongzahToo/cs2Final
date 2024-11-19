﻿using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class GetUsersResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<User> userList { get; set; }
    }
}
