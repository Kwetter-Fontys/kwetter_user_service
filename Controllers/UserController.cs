﻿using Microsoft.AspNetCore.Authorization;
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
        public List<User> GetAllUsersAsync()
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
    }
}
