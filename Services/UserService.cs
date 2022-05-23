using Microsoft.Extensions.Logging;
using UserService.DAL.Repositories;
using UserService.Models;
using UserService.ViewModels;
using System;
using RabbitMQ.Client;
using System.Text;

namespace UserService.Services
{
    public class UserServiceClass
    {
        IUserRepository UserRepository;
        private readonly ILogger _logger;

        public UserServiceClass(IUserRepository userRepo, ILogger<UserServiceClass> logger)
        {
            _logger = logger;
            UserRepository = userRepo;
        }

        public List<UserViewModel> GetAllUsers()
        {
            _logger.LogInformation("Succesfully gotten all users");
            return TransformToViewModelList(UserRepository.GetUsers());
        }

        public List<UserViewModel> GetAllFollowersFromUser(string userId)
        {
            List<User> followers = UserRepository.GetFollowers(userId);
            //Check if empty
            if (!followers.Any())
            {
                _logger.LogWarning("List of empty followers was gotten from user: " + userId);
            }
            else
            {
                _logger.LogInformation("List of " + followers.Count.ToString() + " followers was gotten from user: " + userId);
            }
            return TransformToViewModelList(followers);
        }
        public List<UserViewModel> GetAllFollowingsFromUser(string userId)
        {
            List<User> followings = UserRepository.GetFollowings(userId);
            //Check if empty
            if (!followings.Any())
            {
                _logger.LogWarning("List of empty followings was gotten from user: " + userId);
            }
            else
            {
                _logger.LogInformation("List of " + followings.Count.ToString() + " followings was gotten from user: " + userId);
            }
            return TransformToViewModelList(followings);
        }

        public UserViewModel? GetSingleUser(string id, string userTokenId)
        {
            if (id == userTokenId)
            {
                //Should be done automatically through rabbitmq when a keycloak user is made.
                if (UserRepository.FindUser(id) == null)
                {
                    CreateUser(userTokenId);
                }
            }

            User? user = UserRepository.FindUser(id);
            if (user == null)
            {
                _logger.LogWarning("User with id: " + id + " was not found");
                return null;
            }
            _logger.LogInformation("User with id: " + id + " was found");
            return TransformToViewModel(user);
           
        }

        public UserViewModel EditSingleUser(string userTokenId, User user)
        {
            if (userTokenId == user.Id)
            {
                if (UserRepository.FindUser(user.Id) != null)
                {
                    _logger.LogInformation("User with id: " + userTokenId + " was edited");
                    return TransformToViewModel(UserRepository.EditUser(user));
                }
                else
                {
                    _logger.LogWarning("User with id: " + userTokenId + " not found");
                }
            }
            else
            {
                _logger.LogWarning("User: " + userTokenId + " tried to edit and impersonate user: " + user.Id);
            }
            return TransformToViewModel(user);
        }

        public string FollowUser(string userWantingToFollow, string followedUser)
        {
            //Check if users exist and aren't the same
            if (userWantingToFollow != followedUser && UserRepository.FindUser(userWantingToFollow) != null && UserRepository.FindUser(followedUser) != null)
            {
                FriendsLink? friend = UserRepository.FindFollower(userWantingToFollow, followedUser);
                if (friend == null)
                {
                    UserRepository.FollowUser(new FriendsLink() { UserFollowerId = userWantingToFollow, UserFollowingId = followedUser });
                    _logger.LogInformation("User: {userWantingToFollow} followed user: {followedUser}", userWantingToFollow, followedUser);
                    return "Followed user";
                }
                _logger.LogWarning("User: {userWantingToFollow} already follows user: {followedUser}", userWantingToFollow, followedUser);
                return "Person is already followed";
            }
            _logger.LogWarning("User: {userWantingToFollow} doesn't exist, or is user: {followedUser}", userWantingToFollow, followedUser);
            return "{string: 'test'}";
            //return "Person doesn't exist or is the same user";
        }

        public string UnFollowUser(string userWantingToUnfollow, string followedUser)
        {
            FriendsLink? friend = UserRepository.FindFollower(userWantingToUnfollow, followedUser);
            if (friend != null)
            {
                UserRepository.UnFollowUser(friend);
                _logger.LogInformation("User: {userWantingToUnfollow} unfollowed user: {followedUser}", userWantingToUnfollow, followedUser);
                return "Person unfollowed";
            }
            _logger.LogWarning("User: {userWantingToUnfollow} doesn't exist, or doesn't follow user: {followedUser}", userWantingToUnfollow, followedUser);
            return "{string: 'test'}";
            //return "Person doesn't exist or is not followed";
        }


        //Should be done through keycloak
        public UserViewModel CreateUser(string userTokenId)
        {
            if (UserRepository.FindUser(userTokenId) == null)
            {
                User user = new User("", "") { Id = userTokenId, Location = "", Biography = "", Website = "", Image = "./assets/randomPerson10.jpg" };
                _logger.LogInformation("User: {userTokenId} succesfully created", userTokenId);
                return TransformToViewModel(UserRepository.CreateUser(user));
            }
            _logger.LogWarning("User: {userTokenId} already exists", userTokenId);
            return new UserViewModel();
        }

        public UserViewModel TransformToViewModel(User user)
        {
            return new UserViewModel { Id = user.Id, Location = user.Location, Biography = user.Biography, FirstName = user.FirstName, LastName = user.LastName, Image = user.Image, Website = user.Website };
        }

        public List<UserViewModel> TransformToViewModelList(List<User> users)
        {
            List<UserViewModel> allUsers = new List<UserViewModel>();
            foreach (User user in users)
            {
                allUsers.Add(TransformToViewModel(user));
            }
            return allUsers;
        }

        public void DeleteUser(string userTokenId, User user)
        {
            if (userTokenId == user.Id)
            {
                if (UserRepository.FindUser(user.Id) != null)
                {
                    _logger.LogInformation("User with id: " + userTokenId + " was deleted");
                    DeleteTweetsFromUser(userTokenId);
                    UserRepository.DeleteUser(user);
                }
                else
                {
                    _logger.LogWarning("User with id: " + userTokenId + " not found");
                }
            }
            else
            {
                _logger.LogWarning("User: " + userTokenId + " tried to delete and impersonate user: " + user.Id);
            }
        }

        public void DeleteTweetsFromUser(string userTokenId)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "38.242.248.109",
                UserName = "guest",
                Password = "pi4snc7kpg#77Q#F"

            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello2", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
