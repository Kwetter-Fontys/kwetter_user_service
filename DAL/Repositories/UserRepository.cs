using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext userContext;

        public UserRepository(UserContext context)
        {
            this.userContext = context;
        }
        public List<User> GetUsers()
        {
            return userContext.Users.ToList();
        }
        public List<User> GetFollowers(int id)
        {
            //This returns all the followerId that equal the following id
            List<int> followerIds = userContext.FriendsLinks.Where(f => f.UserFollowingId == id).Select(column=> column.UserFollowerId).ToList();
            //Returns all the users that are followers of the gotten ID
            return userContext.Users.Where(u => followerIds.Contains(u.Id)).ToList();
        }
        public List<User> GetFollowings(int id)
        {
            //This returns all the followingId that equal the follower id
            List<int> followingIds = userContext.FriendsLinks.Where(f => f.UserFollowerId == id).Select(column => column.UserFollowingId).ToList();
            //Returns all the users that are followings of the gotten ID
            return userContext.Users.Where(u => followingIds.Contains(u.Id)).ToList();
        }

        public User GetUser(int id)
        {
            return userContext.Users.Find(id);
        }

        public User EditUser(int id, User user)
        {
            //Should add catches
            userContext.Users.Update(user);
            userContext.SaveChanges();
            return user;
        }

        public void AddUser(User user)
        {
        }

        public void DeleteUser(User user)
        {
        }


    }
}
