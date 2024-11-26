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
                else if (ResponseModel.status == false)
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

    }
}
