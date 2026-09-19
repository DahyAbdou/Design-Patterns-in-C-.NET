using Application.Abstraction.Repository;
using Application.Abstraction.Services;
using Application.Abstraction.UnitOfWork;
using Application.Request.DoctorJobTitle;
using Application.Response.DoctorJobTitle;
using Application.Response.User;
using AutoMapper;
using Domain.Entities;
using Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.Enums;

namespace Application.Implementation.Services
{
    public class DoctorJobTitleService:IDoctorJobTitleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DoctorJobTitleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        }

        public async Task<DoctorJobTitleDto> GetDoctorJobTitleAsync(DoctorJobTitleRequest request)
        {
            var doctorJobTitle = await _unitOfWork.DoctorJobTitleRepository.GetDoctorJobTitleByNameAsync(request.JobTitle);

            if (doctorJobTitle == null)
                throw new CustomHttpException(System.Net.HttpStatusCode.NotFound, "doctor Job Title is not found");


            var mappedUser = _mapper.Map<DoctorJobTitle, DoctorJobTitleDto>(doctorJobTitle);

            return mappedUser;
        }
    }
}
