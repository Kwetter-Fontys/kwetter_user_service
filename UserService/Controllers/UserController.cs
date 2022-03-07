using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DAL;
using UserService.DAL.Repositories;
using UserService.Models;


namespace UserService.Controllers
{
    [Route("api/usercontroller")]
    [ApiController]
    public class UserController
    {

        IUserRepository UserRepository = null;
        public UserController(IUserRepository useRepo)
        {
            UserRepository = useRepo;
        }

        [HttpGet] // GET /api/usercontroller
        public List<User> GetAllUsers()
        {
            return UserRepository.GetUsers();
        }

        [HttpGet("{id}")]   // GET /api/usercontroller/xyz
        public User GetProduct(string id)
        {
            return UserRepository.GetUser(id);
        }
    }
}
