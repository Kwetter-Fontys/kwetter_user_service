using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DAL.Repositories;
using UserService.Models;
using UserService.ViewModels;

namespace UserService.Services
{
    public class UserServiceClass
    {
        IUserRepository UserRepository;

        public UserServiceClass(IUserRepository userRepo)
        {
            UserRepository = userRepo;
        }

        public List<UserViewModel> GetAllUsers()
        {
            return TransformToViewModelList(UserRepository.GetUsers());
        }

        public List<UserViewModel> GetAllFollowersFromUser(string userId)
        {
            return TransformToViewModelList(UserRepository.GetFollowers(userId));
        }
        public List<UserViewModel> GetAllFollowingsFromUser(string userId)
        {
            return TransformToViewModelList(UserRepository.GetFollowings(userId));
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
                return null;
            }
            return TransformToViewModel(user);
           
        }

        public UserViewModel EditSingleUser(string userTokenId, User user)
        {
            if (userTokenId == user.Id)
            {
                if (UserRepository.FindUser(user.Id) != null)
                {
                    return TransformToViewModel(UserRepository.EditUser(user));
                }
            }
            return TransformToViewModel(user);
        }

        public string FollowUser(string userWantingToFollow, string followedUser)
        {
            //Check if users exist and aren't the same
            if (userWantingToFollow != followedUser && UserRepository.FindUser(userWantingToFollow) != null && UserRepository.FindUser(followedUser) != null)
            {
                FriendsLink? friend = UserRepository.FindFollower(followedUser, userWantingToFollow);
                if (friend == null)
                {
                    UserRepository.FollowUser(new FriendsLink() { UserFollowerId = userWantingToFollow, UserFollowingId = followedUser });
                    return "User followed";
                }
            }
            return "Placeholder error";
        }

        public string UnFollowUser(string userWantingToUnfollow, string userTokenId)
        {
            FriendsLink? friend = UserRepository.FindFollower(userWantingToUnfollow, userTokenId);
            if (friend != null)
            {
                UserRepository.UnFollowUser(friend);
            }
            return userWantingToUnfollow;
        }

        public UserViewModel CreateUser(string userTokenId)
        {
            if (UserRepository.FindUser(userTokenId) == null && userTokenId != null)
            {
                User user = new User("", "") { Id = userTokenId, Location = "", Biography = "", Website = "", Image = "./assets/randomPerson10.jpg" };
                return TransformToViewModel(UserRepository.CreateUser(user));
            }
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
    }
}
