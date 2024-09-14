using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IProjectServices;
using Application.Request;
using Application.Response;
using FluentValidation;
using Application.Validators;
using Application.Interfaces.IValidator;



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

            var project = new Domain.Entities.Project
            {
                ProjectName = request.Name,
                StartDate = request.Start,
                EndDate = request.End,
                ClientID = request.Client,
                CampaignType = request.CampaignType,
                CreateDate = DateTime.Now,
            };

            await _projectCommand.InsertProject(project);

            Domain.Entities.Project uProject = await _projectQuery.GetProjectById(project.ProjectID);

            var projectDetails =  new ProjectDetails
            {
                Data = new Response.Project
                {
                    Id = uProject.ProjectID,
                    Name = uProject.ProjectName,
                    Start = uProject.StartDate,
                    End = uProject.EndDate,
                    Client = new Clients
                    {
                        Id = uProject.Clients.ClientID,
                        Name = uProject.Clients.Name,
                        Email = uProject.Clients.Email,
                        Company = uProject.Clients.Company,
                        Phone = uProject.Clients.Phone,
                        Address = uProject.Clients.Address
                    },
                    CampaignType = new GenericResponse
                    {
                        Id = uProject.CampaignTypes.Id,
                        Name = uProject.CampaignTypes.Name
                    }
                },
                Interactions = new List<Interactions>(),
                Tasks = new List<Tasks>()
            };

            return projectDetails;
        }

       
    }


}
