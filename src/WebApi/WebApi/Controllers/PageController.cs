namespace WebApi.Controllers
{
    using System;
    using Domain;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Role.Admin)]
    [Route("api/[controller]/[action]")]
    public class PageController : ControllerBase
    {
        [HttpGet("Get/1")]
        [Authorize(Roles = Role.PageOne)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult PageOne()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Get/2")]
        [Authorize(Roles = Role.PageTwo)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult PageTwo()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Get/3")]
        [Authorize(Roles = Role.PageThree)]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult PageThree()
        {
            throw new NotImplementedException();
        }
    }
}