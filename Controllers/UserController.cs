using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DAL.Repositories;
using UserService.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace UserService.Controllers
{
    [Authorize]
    [Route("api/usercontroller")]
    [ApiController]
    public class UserController : ControllerBase
    {
        JwtTokenHelper jwtTokenHelper;
        readonly IUserRepository UserRepository;
        public UserController(IUserRepository useRepo)
        {
            jwtTokenHelper = new JwtTokenHelper();
            UserRepository = useRepo;
        }

        //Only admins
        [HttpGet] // GET /api/usercontroller
        public List<User> GetAllUsers()
        {
            return UserRepository.GetUsers();
        }

        [HttpGet("followers/{id}")] // GET /api/usercontroller/followers/xyz
        public List<User> GetFollowers(string id)
        {
            return UserRepository.GetFollowers(id);
        }
        [HttpGet("followings/{id}")] // GET /api/usercontroller/followings/xyz
        public List<User> GetFollowings(string id)
        {
            return UserRepository.GetFollowings(id);
        }

        [HttpGet("{id}")]   // GET /api/usercontroller/xyz
        public User GetSingleUser(string id)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            if (id == userTokenId)
            {
                if (UserRepository.GetUser(id) == null)
                {
                    UserRepository.CreateUser(userTokenId);
                }

            }
                return UserRepository.GetUser(id);
        }

        //Only that user or admin
        [HttpPut("{id}")]   // PUT /api/usercontroller/xyz
        public User EditSingleUser(User user)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return UserRepository.EditUser(userTokenId, user);
        }

        //Only that user or admin
        [HttpPut("follow/{userBeingFollowed}")]   // PUT /api/usercontroller/follow/xyz
        public string FollowUser(string userBeingFollowed)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            UserRepository.FollowUser(userTokenId, userBeingFollowed);
            return userBeingFollowed;
        }

        //Only that user or admin
        [HttpPut("unfollow/{userBeingFollowed}")]   // PUT /api/usercontroller/unfollow/xyz
        public string unFollowUser(string userBeingFollowed)
        {
            Console.WriteLine("yooooooooooooooooooooooooooooyoooooooooooooooooooooooooooooooooooooooooooooooooo");
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            UserRepository.unFollowUser(userTokenId, userBeingFollowed);
            return userBeingFollowed;
        }
    }
}
