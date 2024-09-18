using Application.Exceptions;
using Application.Interfaces.IServices.IProjectServices;
using Application.Models;
using Application.Request;
using Application.Response;
using Application.UseCase;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Rocio_Kreick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectGetServices _projectGetService;
        private readonly IProjectPostServices _projectPostService;
        private readonly IProjectPatchServices _projectPatchService;
        private readonly IProjectPutServices _projectPutService;
        

        public ProjectController(IProjectGetServices projectGetService, IProjectPostServices projectPostService, IProjectPatchServices projectPatchService, IProjectPutServices projectPutService)
        {
            _projectGetService = projectGetService;
            _projectPostService = projectPostService;
            _projectPatchService = projectPatchService;
            _projectPutService = projectPutService;

        }

        /// <summary>
        /// Retrieves a list of projects based on the provided filters such as
        /// project name, campaign type, client, with optional pagination
        /// parameters.
        /// </summary>
        /// <param name="name">Optional. Filter by project name.</param>
        /// <param name="campaign">Optional. Filter by campaign type ID.</param>
        /// <param name="client">Optional. Filter by client ID.</param>
        /// <param name="offset">Optional. Skip the specified number of records (used for pagination).</param>
        /// <param name="size">Optional. Limit the number of records returned (used for pagination).</param>
        /// <response code="200">Success</response>
        /// /// <returns>A list of projects that match the specified filters.</returns>

        [HttpGet]
        [ProducesResponseType(typeof(List<Project>), 200)]
        public async Task<ActionResult> GetProjects(
            [FromQuery] string? name,
            [FromQuery] int? campaign,
            [FromQuery] int? client,
            [FromQuery] int? offset,
            [FromQuery] int? size)
        {
            var result = await _projectGetService.GetProjects(name, campaign, client, offset, size);
            return new JsonResult(result) { StatusCode = 200 };
        }


        /// <summary>
        /// Creates a new project with the specified details.
        /// </summary> 
        /// <param name="request">The details of the project to be created.</param>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectDetails), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> CreateProject(ProjectRequest request)
        {
            try
            {
                var result = await _projectPostService.CreateProject(request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (ValidationException ex)
            {
                return BadRequest (ex.Errors);
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific project by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <response code="200">Success</response>
        /// <returns>The project details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDetails), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public async Task<IActionResult> GetProjectById([FromRoute] Guid id)
        {
            try
            {
                var result = await _projectGetService.GetProjectById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }            
            catch (NotFoundException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 404 };               
            }
        }


        /// <summary>
        /// Adds a new interaction to an existing project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <param name="request">The details of the interaction to be added.</param>
        /// <response code="200">Success</response>
        /// <returns>The project interactions.</returns>
        [HttpPatch("{id}/interactions")]
        [ProducesResponseType(typeof(Interactions), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> AddInteraction([FromRoute] Guid id, [FromBody] InteractionsRequest request)
        {
            try
            {
                var result = await _projectPatchService.AddInteraction(id, request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }


        /// <summary>
        /// Adds a new task to an existing project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <param name="request">The details of the task to be added.</param>
        /// <response code="200">Success</response>
        /// <returns>The project interactions.</returns>
        [HttpPatch("{id}/tasks")]
        [ProducesResponseType(typeof(Tasks), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> AddTask([FromRoute] Guid id, [FromBody] TasksRequest request)
        {
            try
            {
                var result = await _projectPatchService.AddTask(id, request);
                return new JsonResult(result) { StatusCode = 201 };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }


        /// <summary>
        /// Updates an existing task with the specified details.
        /// </summary>
        /// <param name="id">The unique identifier of the task to be updated.</param>
        /// <param name="request">The updated details of the task.</param>
        /// <response code="200">Success</response>         
        [HttpPut("/api/v1/Tasks/{id}")]
        [ProducesResponseType(typeof(Tasks), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] TasksRequest request)
        {
            try
            {
                var result = await _projectPutService.UpdateTask(id, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }


    }
}
