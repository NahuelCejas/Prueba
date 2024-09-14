using Application.Interfaces.IServices.ICampaignTypeServices;
using Application.Response;
using Application.UseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Rocio_Kreick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignTypeController : ControllerBase
    {
        private readonly ICampaignTypeGetServices _services;

        public CampaignTypeController(ICampaignTypeGetServices services)
        {
            _services = services;
        }

        /// <summary>
        /// Retrieves a list of all  campaign types.
        ///</summary>
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
