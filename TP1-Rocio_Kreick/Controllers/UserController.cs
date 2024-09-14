using Application.Interfaces.IServices.IUserServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Rocio_Kreick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserGetServices _services;

        public UserController(IUserGetServices services)
        {
            _services = services;
        }

        /// <summary>
        /// Retrieves a list of all users.
        ///</summary>
        /// <response code="200"> Success </response>

        [HttpGet]
        [ProducesResponseType(typeof(Application.Response.Users[]), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetAll();
            return new JsonResult(result);
        }
    }
}
