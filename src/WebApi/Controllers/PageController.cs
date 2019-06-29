namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    public class PageController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "PageOne,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult PageOne()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "PageTwo,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult PageTwo()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "PageThree,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult PageThree()
        {
            return Ok();
        }
    }
}