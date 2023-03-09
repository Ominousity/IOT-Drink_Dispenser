using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class DrinkValidator : AbstractValidator<drink>
    {
        public DrinkValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AlcCL).GreaterThan(0.5f);
        }
    }
}
