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
            return TransformToViewModelList(UserRepository.GetFollowers(userId));
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
                return TransformToViewModel(UserRepository.EditUser(user));
            }
            return TransformToViewModel(user);
        }

        public string FollowUser(string userBeingFollowed, string userTokenId)
        {
            FriendsLink? friend = UserRepository.FindFollower(userTokenId, userBeingFollowed);
            if (friend == null)
            {
                UserRepository.FollowUser(new FriendsLink(){ UserFollowerId = userBeingFollowed, UserFollowingId = userTokenId });
            }
            return userBeingFollowed;
        }

        public string UnFollowUser(string userBeingFollowed, string userTokenId)
        {
            FriendsLink? friend = UserRepository.FindFollower(userTokenId, userBeingFollowed);
            if (friend != null)
            {
                UserRepository.UnFollowUser(friend);
            }
            return userBeingFollowed;
        }

        public UserViewModel CreateUser(string userTokenId)
        {
            User user = new User("", "") { Id = userTokenId, Location = "", Biography = "", Website = "", Image = "./assets/randomPerson10.jpg" };
            return TransformToViewModel(UserRepository.CreateUser(user));
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
