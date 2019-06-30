namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application;
    using Domain;
    using Domain.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Role.Admin)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCreate<UserDto> _userCreate;
        private readonly IUserDelete<UserDto> _userDelete;
        private readonly IUserFind<UserFindDto> _userFind;
        private readonly IUserUpdate<UserDto> _userUpdate;

        public UserController(IUserFind<UserFindDto> userFind, IUserCreate<UserDto> userCreate, IUserDelete<UserDto> userDelete,
            IUserUpdate<UserDto> userUpdate)
        {
            _userFind = userFind;
            _userCreate = userCreate;
            _userDelete = userDelete;
            _userUpdate = userUpdate;
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
        public ActionResult<IEnumerable<UserFindDto>> GetAll()
        {
            return new ActionResult<IEnumerable<UserFindDto>>(_userFind.GetAll());
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
        public async Task<ActionResult<UserFindDto>> Get(string id)
        {
            var result = new ActionResult<UserFindDto>(await _userFind.GetById(id));

            if (result.Value == null) return NotFound("User not found");

            return result;
        }

        // GET api/User/GetByUsername/{username}
        /// <summary>
        ///     Get user using an username
        /// </summary>
        /// <returns>A user</returns>
        /// <response code="201">Return a user</response>
        /// <response code="400">Username null or Empty</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{username}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserFindDto>> GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return BadRequest("Username null or Empty");

            var result = new ActionResult<UserFindDto>(await _userFind.GetByUsername(username));

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
            if (dto != null && string.IsNullOrWhiteSpace(dto.Id)) return BadRequest("Id null or Empty");

            if (dto != null && string.IsNullOrWhiteSpace(dto.Username)) return BadRequest("Username null or Empty");

            if (dto != null && string.IsNullOrWhiteSpace(dto.Password)) return BadRequest("Password null or Empty");

            try
            {
                var user = await _userCreate.Execute(dto);
                return user;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/User/
        /// <summary>
        ///     Modify a User
        /// </summary>
        /// <returns>The user modified</returns>
        /// <response code="201">User modified</response>
        /// <response code="400">Incorrect parameters</response>
        /// <response code="401">Not authorized</response>
        /// <response code="409">Conflict</response>
        [HttpPut]
        public async Task<ActionResult<UserDto>> Put([FromBody] UserDto entity)
        {
            if (entity != null && string.IsNullOrWhiteSpace(entity.Id)) return BadRequest("Id null or Empty");

            try
            {
                var user = await _userUpdate.Execute(entity);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/User/
        /// <summary>
        ///     Delete a User
        /// </summary>
        /// <returns>The user deleted</returns>
        /// <response code="201">User deleted</response>
        /// <response code="400">Incorrect parameters</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userFind.GetById(id);

            if (user == null) return NotFound("User not found");

            try
            {
                await _userDelete.Execute(new UserDto {Id = id});
                return Ok();
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}