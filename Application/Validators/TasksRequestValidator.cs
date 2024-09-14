using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class TasksRequestValidator : AbstractValidator<TasksRequest>
    {
        public TasksRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name is required");

            RuleFor(x => x.User)
                .NotEmpty()
                .WithMessage("User is required")
                .InclusiveBetween(1, 5)
                .WithMessage("User must be a number between 1 and 5.");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .InclusiveBetween(1, 5)
                .WithMessage("Status must be a number between 1 and 5.");

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .WithMessage("Due Date is required.")
                .GreaterThan(DateTime.Now)
                .WithMessage("DueDate must be in the future.");
        }
    }
}
