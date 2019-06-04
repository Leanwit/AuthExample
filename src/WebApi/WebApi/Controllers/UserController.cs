namespace WebApi.Controllers
{
    using System.Collections.Generic;
    using Application;
    using Domain.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserFinder _userFinder;

        public UserController(IUserFinder userFinder)
        {
            this._userFinder = userFinder;
        }

        // GET api/account
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns>A users list</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="401">Not authorized</response>
        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            return new ActionResult<IEnumerable<UserDto>>(this._userFinder.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetByUsername(string username)
        {
            return new ActionResult<UserDto>(this._userFinder.GetByUsername(username));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}