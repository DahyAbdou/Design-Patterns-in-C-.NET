using Application.Abstraction;
using Application.Abstraction.Repository;
using Application.Abstraction.UnitOfWork;
using Infrastructure.Implementation.Repository;
using Infrastructure.Implementation.UnitOfWork;
using Infrastructure.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Dependency
{
    public static class ServicesContainer
    {
        public static void AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAppResourceService, AppResourceService>();

            services.AddSingleton<IUserRepository, UserDapperRepository>();
            services.AddTransient<IDoctorJobTitleRepository, DoctorJobTitleRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
