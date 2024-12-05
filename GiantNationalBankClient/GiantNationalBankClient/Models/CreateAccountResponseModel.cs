using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiantNationalBankClient.Models
{
    public class CreateAccountResponseModel
    {
        public bool Status {  get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } 
        public Account Account { get; set; }
    }
}