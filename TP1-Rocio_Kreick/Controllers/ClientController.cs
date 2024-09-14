using Application.Models;
using Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Exceptions;
using Application.Interfaces.IServices.IClientServices;

namespace TP1_Rocio_Kreick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientGetServices _clientGetService;
        private readonly IClientPostServices _clientPostService;

        public ClientController(IClientGetServices clientGetService, IClientPostServices clientPostService)
        {
            _clientGetService = clientGetService;
            _clientPostService = clientPostService;
        }


        /// <summary>
        /// Retrieves a list of all clients.
        ///</summary>
        /// <response code="200"> Success </response>

        [HttpGet]
        [ProducesResponseType(typeof(Clients[]), 200)]

        public async Task<IActionResult> GetAll()
        {
            var result = await _clientGetService.GetAll();

            return new JsonResult(result) { StatusCode = 200 };
        }

        /// <summary>
        /// Creates a new client with the provided details.
        /// </summary> 
        /// <param name="request">The details of the client to be created.</param>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(Clients), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> CreateClient(ClientsRequest request)
        {
            try
            {
                var result = await _clientPostService.CreateClient(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
