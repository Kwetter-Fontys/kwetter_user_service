using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DAL.Repositories;
using UserService.Models;

namespace UserTests
{
    internal class MockUserRepository : IUserRepository
    {
        List<User> users;
        List<FriendsLink> friendsLinks;
        public MockUserRepository()
        {
            users = new List<User>
            {
                new User("sebas", "bakker"){ Id = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", Biography= "Student at Fontys Hogeschool Eindhoven. Creator of Kwetter and everything else.", Location = "Amsterdam", Website = " www.kwetter.nl", Image = "./assets/test.jpg"},
                new User("Sebas2", "Bakker"){ Id = "c888f6c2-d4ce-442f-b630-52a91150f22a", Biography= "Hallo2", Location = "Papendrecht2", Website = "utube.com2", Image = "./assets/randomPerson1.png" },
                new User("Rick", "Paans"){ Id = "1", Biography= "Hallo3", Location = "Papendrecht3", Website = "utube.com3", Image = "./assets/randomPerson2.png" },
                new User("Pim", "Paans"){ Id = "2", Biography= "Hallo4", Location = "Papendrecht4", Website = "utube.com4", Image = "./assets/randomPerson3.png" },
                new User("Sebas5", "Bakker"){ Id = "3", Biography= "Hallo5", Location = "Papendrecht5", Website = "utube.com5", Image = "./assets/randomPerson4.png" },
                new User("Sebas6", "Bakker"){ Id = "4", Biography= "Hallo6", Location = "Papendrecht6", Website = "utube.com6", Image = "./assets/randomPerson5.png" },
                new User("Sebas7", "Bakker"){ Id = "5", Biography= "Hallo7", Location = "Papendrecht7", Website = "utube.com7", Image = "./assets/randomPerson6.png" }
            };

            friendsLinks = new List<FriendsLink>
            {
                new FriendsLink{UserFollowerId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", UserFollowingId = "c888f6c2-d4ce-442f-b630-52a91150f22a"},
                new FriendsLink{UserFollowerId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", UserFollowingId = "3"},
                new FriendsLink{UserFollowerId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", UserFollowingId = "4"},
                new FriendsLink{UserFollowerId = "c888f6c2-d4ce-442f-b630-52a91150f22a", UserFollowingId = "4"},
            };
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public List<User> GetFollowers(string id)
        {
            //This returns all the followerId that equal the following id
            List<string> followerIds = friendsLinks.Where(f => f.UserFollowingId == id).Select(column => column.UserFollowerId).ToList();
            //Returns all the users that are followers of the gotten ID
            return users.Where(u => followerIds.Contains(u.Id)).ToList();
        }

        public List<User> GetFollowings(string id)
        {
            //This returns all the followingId that equal the follower id
            List<string> followingIds = friendsLinks.Where(f => f.UserFollowerId == id).Select(column => column.UserFollowingId).ToList();
            //Returns all the users that are followings of the gotten ID
            return users.Where(u => followingIds.Contains(u.Id)).ToList();
        }

        public User? FindUser(string id)
        {
            return users.Find(u => u.Id == id);
        }

        public User EditUser(User user)
        {
            int index = users.FindIndex(users => users.Id == user.Id);
            users[index] = user;
            return users[index];
        }

        public User CreateUser(User user)
        {
            users.Add(user);
            return user;
        }

        public FriendsLink? FindFollower(string userWantingToFollow, string userBeingFollowed)
        {
            return friendsLinks.Where(friends => friends.UserFollowerId == userWantingToFollow && friends.UserFollowingId == userBeingFollowed).FirstOrDefault();
        }

        public void FollowUser(FriendsLink friendsLink)
        {
            friendsLinks.Add(friendsLink);
        }

        public void UnFollowUser(FriendsLink friendsLink)
        {
            friendsLinks.Remove(friendsLink);
        }

        public void DeleteUser(User user)
        {
            User? deleteUser = FindUser(user.Id);
            users.Remove(deleteUser);
        }
    }
}
