using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DAL.Repositories;
using UserService.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using UserService.Services;
using UserService.ViewModels;
using Microsoft.Extensions.Logging;


namespace UserService.Controllers
{
    [Authorize]
    [Route("api/usercontroller")]
    [ApiController]
    public class UserController : ControllerBase
    {
        JwtTokenHelper jwtTokenHelper;
        readonly IUserService userService;
        private readonly ILogger _logger;
        public UserController(IUserService userServ, ILogger<UserController> logger)
        {
            jwtTokenHelper = new JwtTokenHelper();
            userService = userServ;
            _logger = logger;
        }
  
        [HttpGet] // GET /api/usercontroller
        public List<UserViewModel> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers() was called");
            return userService.GetAllUsers();
        }

        [HttpGet("followers/{userid}")] // GET /api/usercontroller/followers/xyz
        public List<UserViewModel> GetFollowers(string userid)
        {
            _logger.LogInformation("GetFollowers() was called by user: {userid}", userid);
            return userService.GetAllFollowersFromUser(userid);
        }
        [HttpGet("followings/{userid}")] // GET /api/usercontroller/followings/xyz
        public List<UserViewModel> GetFollowings(string userid)
        {
            _logger.LogInformation("GetFollowings() was called by user: {userid}", userid);
            return userService.GetAllFollowingsFromUser(userid);
        }

        [HttpGet("{userid}")]   // GET /api/usercontroller/xyz
        public UserViewModel? GetSingleUser(string userid)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            _logger.LogInformation("GetSingleUser() was called by user: {userTokenId}", userTokenId);
            return userService.GetSingleUser(userid, userTokenId);
        }

        //Only that user or admin
        [HttpPut("{id}")]   // PUT /api/usercontroller/xyz
        //Maybe change parameter to view model too.
        public UserViewModel EditSingleUser(User user)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            _logger.LogInformation("EditSingleUser() was called by user: {userTokenId}", userTokenId);
            return userService.EditSingleUser(userTokenId, user);
        }

        //Only that user or admin
        [HttpDelete("{userId}")]   // Delete /api/usercontroller/xyz
        //Maybe change parameter to view model too.
        public string DeleteUser (string userId)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            _logger.LogInformation("DeleteUser() was called by user: {userTokenId}", userTokenId);
            userService.DeleteUser(userTokenId, userId);
            return userId;
        }


        //Only that user or admin
        [HttpPut("follow/{followedUser}")]   // PUT /api/usercontroller/follow/xyz
        public string FollowUser(string followedUser)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            _logger.LogInformation("FollowUser() was called by user: {userTokenId}", userTokenId);
            userService.FollowUser(userTokenId, followedUser);
            return followedUser;
        }

        //Only that user or admin
        [HttpPut("unfollow/{userBeingFollowed}")]   // PUT /api/usercontroller/unfollow/xyz
        public string UnFollowUser(string userBeingFollowed)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            _logger.LogInformation("UnFollowUser() was called by user: {userTokenId}", userTokenId);
            userService.UnFollowUser(userTokenId, userBeingFollowed);
            return userBeingFollowed;
        }
    }
}
