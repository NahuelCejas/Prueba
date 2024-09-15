using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IProjectServices;
using Application.Request;
using Application.Response;
using FluentValidation;
using Application.Validators;
using Application.Interfaces.IValidator;
using Application.Exceptions;



namespace Application.UseCase.ProjectServices
{
    public class ProjectPostServices : IProjectPostServices
    {
        private readonly IProjectQuery _projectQuery;
        private readonly IProjectCommand _projectCommand;
        private readonly IValidatorHandler<ProjectRequest> _validator;


        public ProjectPostServices(IProjectQuery query, IProjectCommand command, IValidatorHandler<ProjectRequest> validator)
        {
            _projectQuery = query;
            _projectCommand = command;
            _validator = validator;
        }

        public async Task<ProjectDetails> CreateProject(ProjectRequest request)
        {
            await _validator.Validate(request);

            var project = MapRequestToProject(request);
            await _projectCommand.InsertProject(project);

            var uProject = await _projectQuery.GetProjectById(project.ProjectID);
            if (uProject == null)
            {
                throw new NotFoundException("Project not found");
            }

            var projectDetails = MapProjectToResponse(uProject);
            return projectDetails;
        }

        private static Domain.Entities.Project MapRequestToProject(ProjectRequest request)
        {
            return new Domain.Entities.Project
            {
                ProjectName = request.Name!,
                StartDate = request.Start,
                EndDate = request.End,
                ClientID = request.Client,
                CampaignType = request.CampaignType,
                CreateDate = DateTime.Now,
            };
        }

        private static ProjectDetails MapProjectToResponse(Domain.Entities.Project project)
        {
            return new ProjectDetails
            {
                Data = new Response.Project
                {
                    Id = project.ProjectID,
                    Name = project.ProjectName,
                    Start = project.StartDate,
                    End = project.EndDate,
                    Client = project.Clients != null ? new Clients
                    {
                        Id = project.Clients.ClientID,
                        Name = project.Clients.Name,
                        Email = project.Clients.Email,
                        Company = project.Clients.Company,
                        Phone = project.Clients.Phone,
                        Address = project.Clients.Address
                    } : null,
                    CampaignType = project.CampaignTypes != null ? new GenericResponse
                    {
                        Id = project.CampaignTypes.Id,
                        Name = project.CampaignTypes.Name
                    } : null
                },
                Interactions = new List<Interactions>(),
                Tasks = new List<Tasks>()
            };
        }


    }


}
