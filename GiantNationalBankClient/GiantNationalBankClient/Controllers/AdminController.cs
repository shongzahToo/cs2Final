using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using Newtonsoft.Json;
using GiantNationalBankClient.Utility;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace GiantNationalBankClient.Controllers
{
    public class AdminController : Controller
    {
        public async Task<IActionResult> AllAccountsView()
        {
            GetAllAccountsResponseModel ResponseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAllAccounts, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetAllAccountsResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AllAccountsView API " + ex.Message);
            }
            return View(ResponseModel);
        }

        public async Task<IActionResult> ViewAllTransactionsByAccount(Account account)
        {
            GetAccountByIdResponseModel ResponseModel = null;
            try
            {
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(string.Empty, ConstantValues.GetAccountById + "?accountID=" + account.AccountID, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<GetAccountByIdResponseModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AllAccountsView API " + ex.Message);
            }
            return View(ResponseModel);
        }
    }
}
