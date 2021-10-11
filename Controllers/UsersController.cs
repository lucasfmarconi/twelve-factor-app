using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using twelve_factor_app.Services;
using twelve_factor_app.Models;

namespace twelve_factor_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.getUsersFromRemote().Result;
        }

        [HttpPost]
        public IActionResult Post([FromBodyAttribute] User data)
        {
            var usersAndPosts = _userService.makeUsersSendToRemote(10);
            return Ok(usersAndPosts);
        }

    }
}