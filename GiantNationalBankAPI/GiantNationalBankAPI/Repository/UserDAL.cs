﻿using GiantNationalBankAPI.IRepository;
using GiantNationalBankAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GiantNationalBankAPI.Data
{
    #region Constructor
    public class UserDAL : IUser
    {
        //context for the database connection
        private readonly GiantNationalBankContext context;

        private readonly IConfiguration _config;

        public UserDAL(GiantNationalBankContext Context, IConfiguration config)
        {
            context = Context;
            _config = config;
        }
        #endregion


        #region Save User Record Method
        /// <summary>
        /// save the user registration record into database
        /// </summary>
        /// <param name="usermodel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<SaveUserResponse> SaveUserRecord(RegistrationModel usermodel)
        {
            //set up a new object to hold our user information
            User objApplicationUser;

            //set up a response status object to hold the response
            SaveUserResponse res = new SaveUserResponse();

            try
            {
                //use the incoming information to populate our new user
                objApplicationUser = new User
                {
                    FirstName = usermodel.FirstName,
                    LastName = usermodel.LastName,
                    Email = usermodel.Email,
                    Street1 = usermodel.Street1,
                    Street2 = usermodel.Street2,
                    City = usermodel.City,
                    State = usermodel.State,
                    ZipCode = usermodel.ZipCode,
                    Phone = usermodel.Phone


                };

                //populate the Login information
                objApplicationUser.Login = new Login
                {
                    UserName = usermodel.Username,
                    Password = usermodel.Password,
                    UserType = "User"
                };

                //get the ID of the most recent user added to the DB
                var highestID = context.Users.Max(x => x.UserId);

                //set the user ID to be the next ID value
                objApplicationUser.Login.Id = highestID + 1;

                //save this user in the database
                using(var db = new GiantNationalBankContext(_config))
                {
                    db.Users.Add(objApplicationUser);
                    db.SaveChanges();
                }

                //set the success to pass the data back
                res.StatusCode = 200;
                res.Message = "Save Successful";
                res.Status = true;
                res.Data = objApplicationUser;
                res.UserId = objApplicationUser.UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveUserRecord --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 0;
            }
            return res;
        }
        #endregion

        #region Login User Method
        public async Task<LoginResponse> LoginUser(LoginModel tokenData)
        {
            LoginResponse res = new LoginResponse();
            try
            {
                if(tokenData != null)
                {
                    //look for the user in the database
                    var query = context.Logins
                    .Where(x => x.UserName == tokenData.Username && x.Password == tokenData.Password)
                    .FirstOrDefault<Login>();

                    //if query has a result then we have a match
                    if(query != null)
                    {
                        res.Status = true;
                        res.StatusCode = 200;
                        res.Message = "Login Success";

                        //get the user data so we can send it back with
                        UserData user = await GetUserByID(query.Id);

                        if(user != null)
                        {
                            res.UserData = new UserData
                            {
                                UserId = user.UserId,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                Street1 = user.Street1,
                                Street2 = user.Street2,
                                City = user.City,
                                State = user.State,
                                ZipCode = user.ZipCode,
                                Phone = user.Phone
                            };
                        }
                       
                    }
                    else
                    {
                        //the user wasn't found or wasn't a match
                        res.Status = false;
                        res.StatusCode = 500;
                        res.Message = "Login Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginUser --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 500;
            }

            return res;
        }
        #endregion

        #region Find User By Name Method

        public async Task<UserData> FindByName(string username)
        {
            UserData user = null;

            try
            {
                if (username != null)
                {
                    //query the database to find the user who has this username
                    var query = context.Users
                        .Where(x => x.Login.UserName == username).FirstOrDefault<User>();


                    if (query != null)
                    {
                        //set up the object so we can return it
                        user = new UserData
                        {
                            UserId = query.UserId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Email = query.Email,
                            Street1 = query.Street1,
                            Street2 = query.Street2,
                            City = query.City,
                            State = query.State,
                            ZipCode = query.ZipCode,
                            Phone = query.Phone
                        };
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByName --- " + ex.Message);
            }

            return user;
        }
        #endregion

        #region Get All Accounts By User
        public async Task<List<AccountModel>> GetAccountsByUser(int userID)
        {
            //set up a list to hold the accounts
            List<AccountModel> accountList = new List<AccountModel>();

            try
            {
                //query the database
                var accounts = context.Accounts
                    .Where(x => x.UserId == userID).ToList();

                if(accounts != null)
                {
                    foreach (var account in accounts)
                    {
                        //get all of the transactions that belong to this account as a list
                        List<Transaction> transactionList = context.Transactions.Where(x => x.AccountId == account.AccountId).ToList();

                        accountList.Add(new AccountModel()
                        {
                            AccountId = account.AccountId,
                            UserId = account.UserId, 
                            Balance = account.Balance,
                            transactionList = transactionList

                        });
                    }

                    if (accountList.Count == 0)
                    {
                        return null;
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAccountsByUser --- " + ex.Message);
                throw;
            }

            return accountList;
        }
        #endregion

        #region Get User By ID
        public async Task<UserData> GetUserByID(int userID)
        {
            UserData userData = null;

            try
            {
                //query the database
                var user = context.Users.Where(x => x.UserId == userID).FirstOrDefault<User>();

                if (user != null)
                {
                    //set up the user data
                    userData = new UserData
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Street1 = user.Street1,
                        Street2 = user.Street2,
                        City = user.City,
                        State = user.State,
                        ZipCode = user.ZipCode,
                        Phone = user.Phone
                    };

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUserByID --- " + ex.Message);
                throw;
            }

            return userData;
        }
        #endregion

        #region Update User Record Method
        /// <summary>
        /// Update an existing user record
        /// </summary>
        /// <param name="usermodel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<SaveUserResponse> UpdateUserRecord(UserData usermodel)
        {

            //set up a response status object to hold the response
            SaveUserResponse res = new SaveUserResponse();

            try
            {
                using (var db = new GiantNationalBankContext(_config))
                {
                    //find the current user in the database
                    var result = db.Users.SingleOrDefault(u => u.UserId == usermodel.UserId);

                    if (result != null)
                    {
                        //update the data

                        result.FirstName = usermodel.FirstName;
                        result.LastName = usermodel.LastName;
                        result.Email = usermodel.Email;
                        result.Street1 = usermodel.Street1;
                        result.Street2 = usermodel.Street2;
                        result.City = usermodel.City;
                        result.State = usermodel.State;
                        result.ZipCode = usermodel.ZipCode;
                        result.Phone = usermodel.Phone;

                        //save this user in the database
                        db.SaveChanges();

                        //set the success to pass the data back
                        res.StatusCode = 200;
                        res.Message = "Update Successful";
                        res.Status = true;
                        res.Data = result;
                        res.UserId = result.UserId;
                    }
                    else
                    {
                        Console.WriteLine("UpdateUserRecord --- Update Failed");
                        res.Status = false;
                        res.StatusCode = 500;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateUserRecord --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 500;
            }
            return res;
        }
        #endregion

    }
}
