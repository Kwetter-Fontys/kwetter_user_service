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
            //This returns all the followers that equal the id
            return userContext.Users.SelectMany(u => u.Followers).Where(s => s.Id == id).ToList(); 
        }
        public List<User> GetFollowings(int id)
        {
            //This returns all the follwings that equal the id
            return userContext.Users.SelectMany(u => u.Following).Where(s => s.Id == id).ToList();
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
