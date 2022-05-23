using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        public UserServiceClass ExistingService;
        public ILogger<UserServiceClass> logger;

        public UserTest()
        {
            var mock = new Mock<ILogger<UserServiceClass>>();
            logger = mock.Object;
            ExistingService = new UserServiceClass(new MockUserRepository(), logger);
        }

        public UserServiceClass CreateNewService()
        {
            UserServiceClass newSerivce = new UserServiceClass(new MockUserRepository(), logger);
            return newSerivce;
        }

        //Get all users tests
        [TestMethod]
        public void GetAllExistingUsers()
        {
            UserList = ExistingService.GetAllUsers();
            Assert.AreEqual(7, UserList.Count, "List count is not equal, when it should be");
        }

        //Get followers tests
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

        //Get following tests
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

        //Get single user tests
        [TestMethod]
        public void FindExistingUser()
        {
            UserViewModel? user = ExistingService.GetSingleUser(MainUserId, MainUserId);

            Assert.AreEqual(ExistingUser.Biography, user.Biography, "Wrong or no user has been fouind");
        }

        [TestMethod]
        public void FindNonExistingUserWithMathingIdAndUserTokenId()
        {
            UserServiceClass newService = CreateNewService();
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


        //Edit user tests
        [TestMethod]
        public void EditExistingUserWithCorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            UserViewModel? editedUser = newService.EditSingleUser(MainUserId, new User("test", "test") { Id = MainUserId });
            UserViewModel? originalUser = newService.GetSingleUser(MainUserId, MainUserId);

            Assert.AreEqual(originalUser.Id, editedUser.Id, "User ids weren't equal when they should be");
            Assert.AreEqual(originalUser.FirstName, editedUser.FirstName, "User names weren't equal when they should be");
        }

        [TestMethod]
        public void EditExistingUserWithIncorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            UserViewModel? editedUser = newService.EditSingleUser("NonExistant", new User("test", "test") { Id = MainUserId });

            UserViewModel? originalUser = newService.GetSingleUser(MainUserId, MainUserId);

            Assert.AreEqual(originalUser.Id, editedUser.Id, "User ids weren't equal when they should be");
            Assert.AreNotEqual(originalUser.FirstName, editedUser.FirstName, "User names were equal when they shouldnt be");

        }

        [TestMethod]
        public void EditNonExistingUserWithCorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            UserViewModel? editedUser = newService.EditSingleUser("NonExistant", new User("test", "test") { Id = "NonExistant" });
            UserViewModel? originalUser = newService.GetSingleUser("NonExistant", "NonExistant");

            Assert.AreEqual("", originalUser.FirstName, "User was found when there shouldnt be a user");

        }

        [TestMethod]
        public void EditNonExistingUserWithIncorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            UserViewModel? editedUser = newService.EditSingleUser("NonExistant", new User("test", "test") { Id = MainUserId });
            UserViewModel? originalUser = newService.GetSingleUser(MainUserId, MainUserId);

            Assert.AreEqual(originalUser.Id, editedUser.Id, "User ids weren't equal when they should be");
            Assert.AreNotEqual(originalUser.FirstName, editedUser.FirstName, "User names were equal when they shouldnt be");

        }

        //Follow users tests
        [TestMethod]
        public void FollowUserWithCorrectTokens()
        {
            UserServiceClass newService = CreateNewService();
            newService.FollowUser("5", "4");

            UserList = newService.GetAllFollowersFromUser("4");
            Assert.AreEqual(3, UserList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void FollowUserWithInorrectFollowToken()
        {
            UserServiceClass newService = CreateNewService();
            newService.FollowUser("NonExistant", "4");

            UserList = newService.GetAllFollowersFromUser("4");
            Assert.AreNotEqual(3, UserList.Count, "List count is equal, when it shouldn't be");
        }

        [TestMethod]
        public void FollowNonExistingUser()
        {
            UserServiceClass newService = CreateNewService();
            newService.FollowUser("5", "Nonexistant");

            UserList = newService.GetAllFollowersFromUser("Nonexistant");
            Assert.AreEqual(0, UserList.Count, "List count is not 0, when it should be");
        }

        [TestMethod]
        public void FollowSameUserTwice()
        {
            UserServiceClass newService = CreateNewService();
            newService.FollowUser("5", "4");
            newService.FollowUser("5", "4");
            UserList = newService.GetAllFollowersFromUser("4");
            Assert.AreEqual(3, UserList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void FollowSelf()
        {
            UserServiceClass newService = CreateNewService();
            newService.FollowUser("4", "4");
            UserList = newService.GetAllFollowersFromUser("4");
            Assert.AreNotEqual(3, UserList.Count, "List count is equal, when it shouldn't be");
        }

        [TestMethod]
        public void UnfollowWithCorrectTokens()
        {
            UserServiceClass newService = CreateNewService();
            newService.UnFollowUser("7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", "4");

            UserList = newService.GetAllFollowersFromUser("4");
            Assert.AreEqual(1, UserList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void UnfollowExistingUserWithNonexistingUser()
        {
            UserServiceClass newService = CreateNewService();
            newService.UnFollowUser("NonExistant", "4");

            UserList = newService.GetAllFollowersFromUser("4");
            Assert.AreNotEqual(1, UserList.Count, "List count is equal, when it shouldn't be");
        }

        [TestMethod]
        public void UnfollowNonExistingUserWithExistingUser()
        {
            UserServiceClass newService = CreateNewService();
            newService.UnFollowUser("7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", "Nonexistant");

            UserList = newService.GetAllFollowingsFromUser("7cc35fc6-0eaf-4df8-aaef-773077b4f3c9");
            Assert.AreEqual(3, UserList.Count, "List count is not equal, when it should be");
        }

        //Create user
        [TestMethod]
        public void CreateNewUserWithNonexistingId()
        {
            UserServiceClass newService = CreateNewService();
            UserViewModel user = newService.CreateUser("NonExistant");
            UserList = newService.GetAllUsers();
            Assert.AreEqual(8, UserList.Count, "User wasn't sucesfully added to the list of users");
            Assert.AreEqual("NonExistant", user.Id, "User wasn't correctly created");
        }

        [TestMethod]
        public void CreateNewUserWithExistingId()
        {
            UserServiceClass newService = CreateNewService();
            UserViewModel user = newService.CreateUser("4");
            UserList = newService.GetAllUsers();
            Assert.AreEqual(7, UserList.Count, "User was sucesfully added to the list of users when it shouldn't have");
            Assert.AreEqual(null, user.Id, "User was sucesfully created when it shouldn't have");
        }

        //Testing transform view model method
        [TestMethod]
        public void TransformToViewModelCorrectlyTransformsAUser()
        {
            UserViewModel userVm = ExistingService.TransformToViewModel(ExistingUser);
            Assert.AreEqual(ExistingUser.Biography, userVm.Biography, "User was not correctly transformed");
        }

        [TestMethod]
        public void TransformToViewModelListCorrectlyTransformsAListOfUser()
        {
            List<User> usersList = new List<User>() { ExistingUser };
            List<UserViewModel> userVm = ExistingService.TransformToViewModelList(usersList);
            Assert.AreEqual(usersList.Count, userVm.Count, "Userlist was not correctly transformed");
        }


        //Delete user tests
        [TestMethod]
        public void DeleteExistingUserWithCorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            newService.DeleteUser(MainUserId, ExistingUser);
            UserViewModel? deletedUser = newService.GetSingleUser(MainUserId, MainUserId);
            Assert.AreEqual("", deletedUser.FirstName, "User wasn't correctly deleted");
        }

        [TestMethod]
        public void DeleteExistingUserWithIncorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            newService.DeleteUser("", ExistingUser);
            UserViewModel? deletedUser = newService.GetSingleUser(MainUserId, MainUserId);
            Assert.AreEqual(ExistingUser.FirstName, deletedUser.FirstName, "User was deleted when it shouldn't be");

        }

        [TestMethod]
        public void DeleteNonExistingUserWithCorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            newService.DeleteUser("NonExistant", new User("test", "test") { Id = "NonExistant" });
            UserViewModel? deletedUser = newService.GetSingleUser("NonExistant", "NonExistant");
            Assert.AreEqual("", deletedUser.FirstName, "User was found when there shouldnt be a user");

        }

        [TestMethod]
        public void DeleteNonExistingUserWithIncorrectToken()
        {
            UserServiceClass newService = CreateNewService();
            newService.DeleteUser("NonExistant", new User("test", "test") { Id = "NonExistant2" });
            UserViewModel? deletedUser = newService.GetSingleUser("NonExistant", "NonExistant2");
            Assert.AreEqual(null, deletedUser, "User was found when there shouldnt be a user");

        }
    }
}