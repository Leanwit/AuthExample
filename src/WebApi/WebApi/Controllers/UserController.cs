namespace WebApi.Controllers
{
    using Application;
    using Domain.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        // GET api/{id}
        /// <summary>
        /// Get user using an ID
        /// </summary>
        /// <returns>A user</returns>
        /// <response code="201">Return a user</response>
        /// <response code="400">ID invalid</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(long id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var result = new ActionResult<UserDto>(await _userFinder.GetById(id));
            
            if (result.Value == null)
            {
                return NotFound("User not found");
            }

            return result;
        }

        // GET api/GetByUsername
        /// <summary>
        /// Get user using an username
        /// </summary>
        /// <returns>A user</returns>
        /// <response code="201">Return a user</response>
        /// <response code="400">Username null or Empty</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username null or Empty");
            }

            var result = new ActionResult<UserDto>(await this._userFinder.GetByUsername(username));
            
            if (result.Value == null)
            {
                return NotFound("User not found");
            }

            return result;
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