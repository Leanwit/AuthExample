
using WebApi.Application;
using WebApi.Domain;

namespace WebApi.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

//    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IFinder<User> _userFinder;
        
        public UserController(IFinder<User> userFinder)
        {
            this._userFinder = userFinder;
        }
        // GET api/account
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns>A users list</returns>
        /// <response code="201">Returns the newly created item</response>
        [HttpGet]
        [ProducesResponseType(201)]
        public ActionResult<IEnumerable<User>> Get()
        {
            return new ActionResult<IEnumerable<User>>(this._userFinder.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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