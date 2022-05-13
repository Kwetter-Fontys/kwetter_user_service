using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UserService.Models;
using UserService.Services;
using UserService.ViewModels;

namespace UserTests
{
    [TestClass]
    public class UserTest
    {
        public string MainUserId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9";
        public string FakeUserId = "000";
        public List<UserViewModel> UserList;
        public User ExistingUser = 
            new User("sebas", "bakker")
        {
            Id = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9",
            Biography = "Student at Fontys Hogeschool Eindhoven. Creator of Kwetter and everything else.",
            Location = "Amsterdam",
            Website = " www.kwetter.nl",
            Image = "./assets/test.jpg"
        };

        //Used when no modifications are needed.
        public UserServiceClass ExistingService = new UserServiceClass(new MockUserRepository());


        [TestMethod]
        public void GetAllExistingUsers()
        {
            UserList = ExistingService.GetAllUsers();
            Assert.AreEqual(7, UserList.Count, "List count is not equal, when it should be");
        }
    }
}