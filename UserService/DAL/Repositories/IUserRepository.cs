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
        List<User> GetFollowers(int id);
        List<User> GetFollowings(int id);
        User GetUser(int id);

        User EditUser(int id, User user);
    }
}
