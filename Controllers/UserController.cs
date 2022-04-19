using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/usercontroller")]
    [ApiController]
    public class UserController
    {

        IUserRepository UserRepository = null;
        public UserController(IUserRepository useRepo)
        {
            UserRepository = useRepo;
        }

        //Only admins
        [HttpGet] // GET /api/usercontroller
        public List<User> GetAllUsers()
        {
            return UserRepository.GetUsers();
        }

        [HttpGet("followers/{id}")] // GET /api/usercontroller/followers/xyz
        public List<User> GetFollowers(int id)
        {
            return UserRepository.GetFollowers(id);
        }
        [HttpGet("followings/{id}")] // GET /api/usercontroller/followings/xyz
        public List<User> GetFollowings(int id)
        {
            return UserRepository.GetFollowings(id);
        }

        [HttpGet("{id}")]   // GET /api/usercontroller/xyz
        public User GetSingleUser(int id)
        {
            return UserRepository.GetUser(id);
        }

        //Only that user or admin
        [HttpPut("{id}")]   // PUT /api/usercontroller/xyz
        public User EditSingleUser(int id, User user)
        {
            return UserRepository.EditUser(id, user);
        }
    }
}
