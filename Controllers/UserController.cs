using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DAL.Repositories;
using UserService.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using UserService.Services;
using UserService.ViewModels;

namespace UserService.Controllers
{
    [Authorize]
    [Route("api/usercontroller")]
    [ApiController]
    public class UserController : ControllerBase
    {
        JwtTokenHelper jwtTokenHelper;
        UserServiceClass userService;

        public UserController(IUserRepository useRepo)
        {
            jwtTokenHelper = new JwtTokenHelper();
            userService = new UserServiceClass(useRepo);
        }
  
        [HttpGet] // GET /api/usercontroller
        public List<UserViewModel> GetAllUsers()
        {
            return userService.GetAllUsers();
        }

        [HttpGet("followers/{userid}")] // GET /api/usercontroller/followers/xyz
        public List<UserViewModel> GetFollowers(string userid)
        {
            return userService.GetAllFollowersFromUser(userid);
        }
        [HttpGet("followings/{userid}")] // GET /api/usercontroller/followings/xyz
        public List<UserViewModel> GetFollowings(string userid)
        {
            return userService.GetAllFollowingsFromUser(userid);
        }

        [HttpGet("{userid}")]   // GET /api/usercontroller/xyz
        public UserViewModel? GetSingleUser(string userid)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return userService.GetSingleUser(userid, userTokenId);
        }

        //Only that user or admin
        [HttpPut("{id}")]   // PUT /api/usercontroller/xyz
        //Maybe change parameter to view model too.
        public UserViewModel EditSingleUser(User user)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return userService.EditSingleUser(userTokenId, user);
        }

        //Only that user or admin
        [HttpPut("follow/{followedUser}")]   // PUT /api/usercontroller/follow/xyz
        public string FollowUser(string followedUser)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return userService.FollowUser(userTokenId, followedUser);
        }

        //Only that user or admin
        [HttpPut("unfollow/{userBeingFollowed}")]   // PUT /api/usercontroller/unfollow/xyz
        public string UnFollowUser(string userBeingFollowed)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return userService.UnFollowUser(userTokenId, userBeingFollowed);
        }
    }
}
