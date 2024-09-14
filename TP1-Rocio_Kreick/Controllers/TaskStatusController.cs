using Application.Interfaces.IServices.ITaskStatusServices;
using Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Rocio_Kreick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly ITaskStatusGetServices _services;

        public TaskStatusController(ITaskStatusGetServices services)
        {
            _services = services;
        }

        /// <summary>
        /// Retrieves a list of all task statuses.
        /// </summary>
        /// <response code="200"> Success </response>
        [HttpGet]
        [ProducesResponseType(typeof(GenericResponse[]), 200)]
        public async Task<IActionResult> GetAll() { 
        
            var result = await _services.GetAll();
            return new JsonResult(result);
        }
    }
}
