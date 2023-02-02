using JWT_Practice.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace JWT_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHubClient> _hubContext;
        public SignalController(IHubContext<MessageHub, IMessageHubClient> hubContext)
        {
            this._hubContext = hubContext;
        }

        [HttpPost]
        [Route("sendMessage")]
        public async Task<IActionResult> Send(string message)
        {
            try
            {
                await _hubContext.Clients.All.SendOffersToUser(message);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
