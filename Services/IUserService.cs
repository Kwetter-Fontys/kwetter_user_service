using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;
using UserService.ViewModels;

namespace UserService.Services
{
    public interface IUserService
    {
        public List<UserViewModel> GetAllUsers();


        public List<UserViewModel> GetAllFollowersFromUser(string userId);

        public List<UserViewModel> GetAllFollowingsFromUser(string userId);


        public UserViewModel? GetSingleUser(string id, string userTokenId);


        public UserViewModel EditSingleUser(string userTokenId, User user);


        public string FollowUser(string userWantingToFollow, string followedUser);


        public string UnFollowUser(string userWantingToUnfollow, string followedUser);

        public UserViewModel CreateUser(string userTokenId);


        public UserViewModel TransformToViewModel(User user);


        public List<UserViewModel> TransformToViewModelList(List<User> users);


        public void DeleteUser(string userTokenId, string userId);


        public void DeleteTweetsFromUser(string userTokenId);

    }
}
