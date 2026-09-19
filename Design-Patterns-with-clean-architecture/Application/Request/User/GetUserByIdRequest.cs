using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Request.User
{
    public class GetUserByIdRequest
    {
        public int Id { get; set; }
    }

    public class GetPhysicianByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
    {
        public GetPhysicianByIdRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("id can't be empty, please provide it");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("id can't be less than or equal to zero");
        }
    }
}
