using GiantNationalBankClient.Models;
using GiantNationalBankClient.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GiantNationalBankClient.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {
            return View();
        }

        public async Task<ActionResult> Edit(User user)
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> EditPost(User updatedUser)
        {
            try
            {
                EditUserResponseModel ResponseModel;

                var str = JsonConvert.SerializeObject(updatedUser);

                ServiceHelper objService = new();

                string response = await objService.PostRequest(str, ConstantValues.Updateuser, false, string.Empty);

                ResponseModel = JsonConvert.DeserializeObject<EditUserResponseModel>(response);

                if (ResponseModel == null)
                {

                }
                else if (ResponseModel.Status == false)
                {

                }
                else
                {
                    LoginResponseModel loginResponseModel = new LoginResponseModel();
                    loginResponseModel.UserData = ResponseModel.Data;
                    return View("Views/User/Index.cshtml", loginResponseModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update User Data " + ex.Message);
            }

            return View("");
        }

        /// <summary>
        /// return all trips for a user with user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IActionResult> AllAccountsView([FromQuery(Name = "userId")] int userID)
        {
            GetAllAccountsResponseModel responseModel = null;
            try
            {
                var strSerializedData = string.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.GetRequest(strSerializedData, ConstantValues.GetAccountByUser + "?userID=" + userID, false, string.Empty).ConfigureAwait(true);
                responseModel = JsonConvert.DeserializeObject<GetAllAccountsResponseModel>(response);
                responseModel.UserId = userID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAccountByUser API: " + ex.Message);
            }
            return View(responseModel);
        }

        public async Task<ActionResult> CreateAccount(int userID)
        {
            CreateAccountResponseModel responseModel = new CreateAccountResponseModel();
            try
            {
                var strSerializedData = String.Empty;
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.CreateNewAccount + "?userID=" + userID, false, string.Empty).ConfigureAwait(true);
                responseModel = JsonConvert.DeserializeObject<CreateAccountResponseModel>(response);

            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateNewAccount API: " + ex.Message);
            }
            return RedirectToAction("AllAccountsView", "User", new { userId = responseModel.Account.UserID });
        }

    }
}
