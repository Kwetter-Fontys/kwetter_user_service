using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DAL.Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        List<User> GetFollowers(string id);
        List<User> GetFollowings(string id);
        User? FindUser(string id);
        void DeleteUser(User user);
        User EditUser(User user);
        User CreateUser(User user);

        FriendsLink? FindFollower(string follower, string followed);

        public void FollowUser(FriendsLink friendsLink);

        public void UnFollowUser(FriendsLink friendsLink);
    }
}
