
using FluentValidation;
using Application.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class InteractionsRequestValidator : AbstractValidator<InteractionsRequest>
    {
        public InteractionsRequestValidator()
        {
            RuleFor(x => x.InteractionType)
                .NotEmpty()
                .WithMessage("InteractionType is required.")
                .InclusiveBetween(1, 4)
                .WithMessage("InteractionType must be a number between 1 and 4.");

            RuleFor(x => x.Notes).NotEmpty().WithMessage("Notes is required.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.")
                .GreaterThan(DateTime.Now)
                .WithMessage("Date must be in the future.");
        }
    }
}
