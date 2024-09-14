using Application.Interfaces.IServices.IInteractionTypeServices;
using Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Rocio_Kreick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteractionTypesController : ControllerBase
    {
        private readonly IInteractionTypeGetServices _services;

        public InteractionTypesController(IInteractionTypeGetServices services)
        {
            _services = services;
        }

        /// <summary>
        /// Retrieves a list of all interaction types.
        /// </summary>
        /// <response code="200"> Success </response>

        [HttpGet]
        [ProducesResponseType(typeof(GenericResponse[]), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetAll();
            return new JsonResult(result) { StatusCode = 200 };
        }
    }
}
