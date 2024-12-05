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

    public class HomeController : Controller
    {
        public IConfiguration _configuration;

        public HomeController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel userData)
        {
            if (ModelState.IsValid)
            {
                ProcessForm(userData);
                return View("Views/Home/Login.cshtml");
            }
            else
            {
                return View();
            }
        }




        /// <summary>
        /// Method that handles the registration form 
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<IActionResult> ProcessForm(RegistrationModel userData)
        {
            //use the userData recieved from the form to make an API call that adds
            //the new user to the database


            //hash the user's password which has been collected as plain text
            string hashedPassword = Helper.EncryptCredentials(userData.Password);

            //update the object so this is what is sent to the database
            userData.Password = hashedPassword;

            if(userData.Street2 ==null) userData.Street2 = string.Empty;

            //call the API with the user data
            try
            {
                RegistrationResponseModel ResponseModel = null;
                var strSerializedData = JsonConvert.SerializeObject(userData);
                ServiceHelper objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, ConstantValues.RegisterUser, false, string.Empty).ConfigureAwait(true);
                ResponseModel = JsonConvert.DeserializeObject<RegistrationResponseModel>(response);

                if (ResponseModel == null)
                {
                    //error with the API that caused a null return

                }
                else if (ResponseModel.Status == false)
                {
                    //an error occured

                }
                else
                {
                    //everything is OK - return
                    return View(ResponseModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration API " + ex.Message);
            }
            return View();
        }

        public async Task<IActionResult> Login(LoginModel userData)
        {
            if (userData != null)
            {
                //depending on whether the user has chosen to login as a user or an agent, we'll handle these differently
                if (userData.UserType == "user")
                {
                    //use the userData recieved from the form to make an API call that checks the login data of the user

                    //hash the user's password which has been collected as plain text
                    string hashedPassword = Helper.EncryptCredentials(userData.Password);

                    //update the object so we're not sending plain text password data to our API
                    userData.Password = hashedPassword;

                    //call the API with the user data
                    try
                    {
                        LoginResponseModel ResponseModel = null;
                        var strSerializedData = JsonConvert.SerializeObject(userData);
                        Console.WriteLine(strSerializedData);
                        ServiceHelper objService = new ServiceHelper();
                        string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginUser, false, string.Empty).ConfigureAwait(true);
                        ResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);
                        if (ResponseModel == null)
                        {
                            //error with the API that caused a null return. what happened?  Needs handled
                        }
                        else if (ResponseModel.Status == false)
                        {
                            //did not log in. Maybe a wrong username or password
                            ViewBag.ErrorMessage = "Email or Password Incorrect";
                            return View("Views/Home/Login.cshtml");
                        }
                        else
                        {
                            //everything is OK
                            //the user should now be considered authenticated
                            //create claims details based on the user information
                            var claims = new[] {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim("UserId", ResponseModel.UserID.ToString())
                             };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(10),
                                signingCredentials: signIn);

                            //Take the user to the UserDashboard
                            return View("Views/User/Index.cshtml", ResponseModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ProcessLogin API " + ex.Message);
                    }
                }
                else if (userData.UserType == "admin")
                {
                    //use the userData recieved from the form to make an API call that checks the login data of the agent

                    //hash the user's password which has been collected as plain text
                    string hashedPassword = Helper.EncryptCredentials(userData.Password);

                    //update the object so we're not sending plain text password data to our API
                    userData.Password = hashedPassword;

                    //call the API with the user data
                    try
                    {
                        LoginResponseModel ResponseModel = null;
                        var strSerializedData = JsonConvert.SerializeObject(userData);
                        Console.WriteLine(strSerializedData);
                        ServiceHelper objService = new ServiceHelper();
                        string response = await objService.PostRequest(strSerializedData, ConstantValues.LoginAdmin, false, string.Empty).ConfigureAwait(true);
                        ResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response);
                        if (ResponseModel == null)
                        {
                            Console.WriteLine("response was null");
                        }
                        else if (ResponseModel.Status == false)
                        {
                            Console.WriteLine("response failed");
                            ViewBag.ErrorMessage = "Email or Password Incorrect";
                            return View("Views/Home/Login.cshtml");
                        }
                        else
                        {
                            Console.WriteLine("response succeeded");
                            return View("Views/Admin/Index.cshtml", ResponseModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ProcessLogin API " + ex.Message);
                    }
                }
            }
            return View();
        }

    }
}
