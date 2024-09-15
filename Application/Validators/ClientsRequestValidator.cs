using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class ClientsRequestValidator : AbstractValidator<ClientsRequest>
    {
        public ClientsRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email address is required.");

            RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company is required.");

            RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?\d{10,15}$")  // Validate phone numbers with international format
            .WithMessage("A valid phone number is required. The number must be 10 to 15 digits long, " +
            "with no spaces, and can optionally start with '+'. Examples: +54911345678901, 11345678901");

            RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.");

        }
    }
}
