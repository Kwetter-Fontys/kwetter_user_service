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
        User GetUser(string id);

        User EditUser(User user);
    }
}
