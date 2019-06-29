namespace WebApi.Controllers
{
    using System.Threading.Tasks;
    using Application;
    using Domain.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IUserAuthenticate _userAuthenticate;

        public AccountController(IUserAuthenticate userAuthenticate)
        {
            _userAuthenticate = userAuthenticate;
        }

        [HttpPost]
        public async Task<ActionResult> Authenticate([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password)) return BadRequest("Username and password is required");

            var user = await _userAuthenticate.Authenticate(userDto.Username, userDto.Password);
            if (user == null) return NotFound();

            return Ok();
        }
    }
}