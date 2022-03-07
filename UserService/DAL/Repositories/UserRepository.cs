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
        public User GetUser(string id)
        {
            int intId = Convert.ToInt32(id);
            return userContext.Users.Find(intId);
        }
        public void AddUser(User user)
        {
        }

        public void DeleteUser(User user)
        {
        }


    }
}
