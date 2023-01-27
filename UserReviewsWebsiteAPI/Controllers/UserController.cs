﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserReviewsWebsiteAPI.Database.Models;
using UserReviewsWebsiteAPI.Database.Models.Dtos;
using UserReviewsWebsiteAPI.Services;

namespace UserReviewsWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetUser()
         {
            string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = _service.GetUser(userId);
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult loginUser(LoginDto user)
        {
            string token = _service.GenerateJwt(user);
            return Ok( new TokenDto { Token = token });
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterDto user)
        {
            _service.RegisterUser(user);
            return Ok();
        }
    }
}
