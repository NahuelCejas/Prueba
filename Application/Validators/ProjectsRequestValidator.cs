
using Application.Exceptions;
using Application.Interfaces.IQuery;
using Application.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class ProjectsRequestValidator : AbstractValidator<ProjectRequest>
    {
        private readonly IProjectQuery _projectQuery;
        private readonly IClientQuery _clientQuery;

        public ProjectsRequestValidator(IProjectQuery projectQuery, IClientQuery clientQuery)
        {
            _projectQuery = projectQuery;
            _clientQuery = clientQuery;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MustAsync(BeUniqueName).WithMessage("A project with the same name already exists.");

            RuleFor(x => x.Start).NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.End)
                .NotEmpty()
                .WithMessage("End date is required.") 
                .GreaterThan(DateTime.Now)
                .WithMessage("End date must be in the future.");

            RuleFor(x => x)
                .Must(x => BeValidEndDate(x.Start, x.End))
                .WithMessage("End date must be greater than Start date.");

            RuleFor(x => x.Client)
                .MustAsync(ClientExists).WithMessage("Client with the provided ID does not exist.");

            RuleFor(x => x.CampaignType)
                .InclusiveBetween(1, 4)
                .WithMessage("The campaignType must be a number between 1 and 4.");            
        }


        private async Task<bool> BeUniqueName(string name, CancellationToken token)
        {
            try
            {
                var project = await _projectQuery.GetProjectByName(name);
                return project == null;
            }
            catch (NotFoundException)
            {
                // If the project is not found, we consider the name to be unique
                return true;
            }
        }


        private static bool BeValidEndDate(DateTime? start, DateTime? end) // Check that End is not before Start
        {
            return end > start; 
        }

       
        private async Task<bool> ClientExists(int clientId, CancellationToken cancellationToken) // Check that Client exists
        {
            
            return await _clientQuery.ClientExists(clientId);
        }
    }
}
