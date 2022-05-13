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

        [TestMethod]
        public void GetAllFollowersFromExistingUser()
        {
            UserList = ExistingService.GetAllFollowersFromUser("4");
            Assert.AreEqual(2, UserList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void GetNoFollowersFromNonExistingUser()
        {
            UserList = ExistingService.GetAllFollowersFromUser("NonExisting");
            Assert.AreEqual(0, UserList.Count, "List count is equal, when it should be");
        }

        [TestMethod]
        public void GetAllFollowingsFromExistingUser()
        {
            UserList = ExistingService.GetAllFollowingsFromUser(MainUserId);
            Assert.AreEqual(3, UserList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void GetNoFollowingsFromNonExistingUser()
        {
            UserList = ExistingService.GetAllFollowingsFromUser("NonExisting");
            Assert.AreEqual(0, UserList.Count, "List count is equal, when it should be");
        }

        [TestMethod]
        public void FindExistingUser()
        {
            UserViewModel? user = ExistingService.GetSingleUser(MainUserId, MainUserId);

            Assert.AreEqual(ExistingUser.Biography, user.Biography, "Wrong or no user has been fouind");
        }

        [TestMethod]
        public void FindNonExistingUserWithMathingIdAndUserTokenId()
        {
            UserServiceClass newService = new UserServiceClass(new MockUserRepository());
            //This also creates a user at the moment so will fail when RabbitMQ is added.
            UserViewModel? user = newService.GetSingleUser("NonExistant", "NonExistant");

            Assert.AreNotEqual(null, user, "User wasn't created after finding no user but having matching ids");
        }

        [TestMethod]
        public void FindNonExistingUser()
        {
            UserViewModel? user = ExistingService.GetSingleUser("NonExistant", "AndNonMatching");

            Assert.AreEqual(null, user, "User was found when it should be null");
        }

        [TestMethod]
        public void EditExistingUserWithCorrectToken()
        {
            UserServiceClass newService = new UserServiceClass(new MockUserRepository());
            UserViewModel? user = newService.EditSingleUser(MainUserId, new User("test", "test") { Id = MainUserId});

            Assert.AreEqual(ExistingUser.Id, user.Id, "User ids weren't equal when they should be");
            Assert.AreNotEqual(ExistingUser.FirstName, user.FirstName, "User names were equal when they shouldnt be");
        }

        //IDK AT THE MOMENT
        [TestMethod]
        public void EditExistingUserWithInCorrectToken()
        {
        }
    }
}