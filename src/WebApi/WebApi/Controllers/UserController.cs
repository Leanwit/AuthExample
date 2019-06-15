namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application;
    using Domain.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCreate<UserDto> _userCreate;
        private readonly IUserDelete<UserDto> _userDelete;
        private readonly IUserFind<UserDto> _userFind;


        public UserController(IUserFind<UserDto> userFind, IUserDelete<UserDto> userDelete,
            IUserCreate<UserDto> userCreate)
        {
            _userFind = userFind;
            _userDelete = userDelete;
            _userCreate = userCreate;
        }

        // GET api/User/GetAll
        /// <summary>
        ///     Get All Users
        /// </summary>
        /// <returns>A users list</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="401">Not authorized</response>
        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            return new ActionResult<IEnumerable<UserDto>>(_userFind.GetAll());
        }

        // GET api/User/{id}
        /// <summary>
        ///     Get user using an ID
        /// </summary>
        /// <returns>A user</returns>
        /// <response code="201">Return a user</response>
        /// <response code="400">ID invalid</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> Get(long id)
        {
            if (id <= 0) return BadRequest("Invalid ID");

            var result = new ActionResult<UserDto>(await _userFind.GetById(id));

            if (result.Value == null) return NotFound("User not found");

            return result;
        }

        // GET api/User/GetByUsername
        /// <summary>
        ///     Get user using an username
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
            if (string.IsNullOrWhiteSpace(username)) return BadRequest("Username null or Empty");

            var result = new ActionResult<UserDto>(await _userFind.GetByUsername(username));

            if (result.Value == null) return NotFound("User not found");

            return result;
        }

        // POST api/User/
        /// <summary>
        ///     Create a new User
        /// </summary>
        /// <returns>The user created</returns>
        /// <response code="201">User created</response>
        /// <response code="400">Incorrect parameters</response>
        /// <response code="401">Not authorized</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserDto dto)
        {
            if (dto != null && string.IsNullOrWhiteSpace(dto.Username)) return BadRequest("Username null or Empty");

            if (dto != null && string.IsNullOrWhiteSpace(dto.Password)) return BadRequest("Password null or Empty");

            try
            {
                var user = await _userCreate.Execute(dto);
                return user;
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("Username duplicated");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/User/
        /// <summary>
        ///     Delete a User
        /// </summary>
        /// <returns>The user created</returns>
        /// <response code="201">User deleted</response>
        /// <response code="400">Incorrect parameters</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            if (id <= 0) return BadRequest("Invalid ID");

            //todo Use only delete
            var user = await _userFind.GetById(id);

            if (user == null) return NotFound("User not found");

            try
            {
                await _userDelete.Execute(user);
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}