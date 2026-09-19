using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Repository
{
    public interface IDoctorJobTitleRepository : IGenericRepository<DoctorJobTitle>
    {
        Task<DoctorJobTitle> GetDoctorJobTitleByNameAsync(string name);
    }
}
