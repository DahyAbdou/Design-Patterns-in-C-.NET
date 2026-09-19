using Application.Request.DoctorJobTitle;
using Application.Response.DoctorJobTitle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Services
{
    public interface IDoctorJobTitleService
    {
        Task<DoctorJobTitleDto> GetDoctorJobTitleAsync(DoctorJobTitleRequest request);
    }
}
