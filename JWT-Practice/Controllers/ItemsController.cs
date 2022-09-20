using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        [HttpGet,Authorize(Roles ="Admin")]
        public async Task<ActionResult<List<string>>> items()
        {
            var list = new List<string>()
            {
                "Ansar",
                "Ali",
                "Sidra"
            };
            return Ok(list);
        }
    }
}
