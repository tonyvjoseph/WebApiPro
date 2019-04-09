using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Services
{
    public class UserService
    {
        public UserModel ValidateUser(string email, string password)
        {
            // Here you can write the code to validate
            // User from database and return accordingly
            // To test we use dummy list here
            var userList = GetUserList();
            var user = userList.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public List<UserModel> GetUserList()
        {
            // Create the list of user and return    
            List<UserModel> userList = new List<UserModel>();
            userList.Add(new UserModel { Id = 1, Name = "tony", Email = "tony@ab.com", Password = "abc" });
            userList.Add(new UserModel { Id = 2, Name = "raj", Email = "raj@ab.com", Password = "asd" });
            userList.Add(new UserModel { Id = 3, Name = "kumar", Email = "raj@ab.com", Password = "zxc" });

            return userList;       
        }
    }
}