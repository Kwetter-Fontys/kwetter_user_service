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
        public List<User> GetFollowers(string id)
        {
            //This returns all the followerId that equal the following id
            List<string> followerIds = userContext.FriendsLinks.Where(f => f.UserFollowingId == id).Select(column=> column.UserFollowerId).ToList();
            //Returns all the users that are followers of the gotten ID
            return userContext.Users.Where(u => followerIds.Contains(u.Id)).ToList();
        }
        public List<User> GetFollowings(string id)
        {
            //This returns all the followingId that equal the follower id
            List<string> followingIds = userContext.FriendsLinks.Where(f => f.UserFollowerId == id).Select(column => column.UserFollowingId).ToList();
            //Returns all the users that are followings of the gotten ID
            return userContext.Users.Where(u => followingIds.Contains(u.Id)).ToList();
        }

        public User? FindUser(string id)
        {
           return userContext.Users.Find(id);
        }

        public User EditUser(User user)
        {
            userContext.Users.Update(user);
            userContext.SaveChanges();
            return user;
        }

        public User CreateUser(User user)
        {
            userContext.Add(user);
            userContext.SaveChanges();
            return user;
        }

        public void DeleteUser(User user)
        {
            //Implemented later
        }

        public FriendsLink? FindFollower(string userWantingToFollow, string userBeingFollowed)
        {
            return userContext.FriendsLinks.Where(friends => friends.UserFollowerId == userWantingToFollow && friends.UserFollowingId == userBeingFollowed).First();
        }

        public void FollowUser(FriendsLink friendsLink)
        {
             userContext.FriendsLinks.Add(friendsLink);
             userContext.SaveChanges();
        }

        public void UnFollowUser(FriendsLink friendsLink)
        {
            userContext.FriendsLinks.Remove(friendsLink);
            userContext.SaveChanges();
        }
    }
}
