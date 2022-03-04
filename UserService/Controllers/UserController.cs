using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DAL;
using UserService.Models;


namespace UserService.Controllers
{
    public class UserController
    {
        private readonly UserContext Context;

        public UserController(UserContext context)
        {
            Context = context;
        }
        public List<User> GetAllUsers()
        {
            return Context.Users.ToList();
        }
    }
}
