using Application.Request.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.DoctorJobTitle
{
    public class DoctorJobTitleRequest
    {
        public string JobTitle { get; set; }
    }
    public class GetDoctorJobTitleRequestValidator : AbstractValidator<DoctorJobTitleRequest>
    {
        public GetDoctorJobTitleRequestValidator()
        {
            RuleFor(x => x.JobTitle).NotNull().NotEmpty().WithMessage("Doctor JobTitle can't be empty, please provide it");
           
        }
    }
}
