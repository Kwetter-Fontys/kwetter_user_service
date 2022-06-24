using Microsoft.Extensions.Logging;
using UserService.DAL.Repositories;
using UserService.Models;
using UserService.ViewModels;
using System;
using RabbitMQ.Client;
using System.Text;

namespace UserService.Services
{
    public class UserServiceClass : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly ILogger _logger;
        private IMessageSender messageSender;
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
                _logger.LogWarning("List of empty followers was gotten from user: {userId}" + userId);
            }
            else
            {
                _logger.LogInformation("List of {followers.Count} followers was gotten from user: {userId}", followers.Count, userId);
            }
            return TransformToViewModelList(followers);
        }
        public List<UserViewModel> GetAllFollowingsFromUser(string userId)
        {
            List<User> followings = UserRepository.GetFollowings(userId);
            //Check if empty
            if (!followings.Any())
            {
                _logger.LogWarning("List of empty followings was gotten from user: {userId}", userId);
            }
            else
            {
                _logger.LogInformation("List of {followings.Count}.ToString() followings was gotten from user: {userId}", followings.Count, userId);
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
                _logger.LogWarning("User with id: {id} was not found", id);
                return null;
            }
            _logger.LogInformation("User with id: {id} was found", id);
            return TransformToViewModel(user);
           
        }

        public UserViewModel EditSingleUser(string userTokenId, User user)
        {
            if (userTokenId == user.Id)
            {
                if (UserRepository.FindUser(user.Id) != null)
                {
                    _logger.LogInformation("User with id: {userTokenId} was edited", userTokenId);
                    return TransformToViewModel(UserRepository.EditUser(user));
                }
                else
                {
                    _logger.LogWarning("User with id: {userTokenId} not found", userTokenId);
                }
            }
            else
            {
                _logger.LogWarning("User: {userTokenId} tried to edit and impersonate user: {user.Id}",userTokenId, user.Id);
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
                    return "User followed";
                }
                _logger.LogWarning("User: {userWantingToFollow} already follows user: {followedUser}", userWantingToFollow, followedUser);
                return "Uswer is already followed";
            }
            _logger.LogWarning("User: {userWantingToFollow} doesn't exist, or is user: {followedUser}", userWantingToFollow, followedUser);
            return "User doesn't exist or is the same user";
        }

        public string UnFollowUser(string userWantingToUnfollow, string followedUser)
        {
            FriendsLink? friend = UserRepository.FindFollower(userWantingToUnfollow, followedUser);
            if (friend != null)
            {
                UserRepository.UnFollowUser(friend);
                _logger.LogInformation("User: {userWantingToUnfollow} unfollowed user: {followedUser}", userWantingToUnfollow, followedUser);
                return "User unfollowed";
            }
            _logger.LogWarning("User: {userWantingToUnfollow} doesn't exist, or doesn't follow user: {followedUser}", userWantingToUnfollow, followedUser);
            return "User doesn't exist or is not followed";
        }


        //Should be done through keycloak
        public UserViewModel CreateUser(string userTokenId)
        {
            if (UserRepository.FindUser(userTokenId) == null)
            {
                User user = new User("", "") { Id = userTokenId, Location = "", Biography = "", Website = "", Image = "./assets/defaultPicture.png" };
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
            allUsers = users.Select(x => new UserViewModel 
            { 
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Location = x.Location,
                Website = x.Website,
                Biography = x.Biography,
                Image = x.Image
            }).ToList();
            return allUsers;
        }

        public void DeleteUser(string userTokenId, string userId)
        {
            if (userTokenId == userId)
            {
                User? user = UserRepository.FindUser(userId);
                if (user != null)
                {
                    _logger.LogInformation("User with id: {userTokenId}  was deleted", userTokenId);
                    DeleteTweetsFromUser(userTokenId);
                    UserRepository.DeleteUser(user);
                }
                else
                {
                    _logger.LogWarning("User with id: {userTokenId} not found", userTokenId);
                }
            }
            else
            {
                _logger.LogWarning("User: {userTokenId} tried to delete and impersonate user: {userId}", userTokenId, userId);
            }
        }

        public void DeleteTweetsFromUser(string userTokenId)
        {
            //Calls rabbitMQ
            if (messageSender == null)
            {
                messageSender = new MessageSender();
            }
            messageSender.SendMessage(userTokenId);
        }



        //fh9!x?YN4AQ&!skt
    }
}
