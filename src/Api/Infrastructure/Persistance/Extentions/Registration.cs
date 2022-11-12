using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Extentions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SweetDictionaryContext>(conf =>
            {
                string connStr = configuration["SweetdictionaryDbConnectionString"].ToString();
                conf.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });

            //var seedData = new SeedData();
            //seedData.SeedAsync(configuration).GetAwaiter().GetResult();

            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
            //services.AddScoped<IEntryRepository, EntryRepository>();
            //services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();

            return services;
        }
    }
}
