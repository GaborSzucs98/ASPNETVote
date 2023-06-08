using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MVCapp.Persitence;
using MVCapp.Persitence.Services;
using System.Reflection;

namespace MVCapp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<VotingDbContext>(options =>
            {
				IConfigurationRoot configuration = builder.Configuration;

				options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));

                options.UseLazyLoadingProxies();
            });

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 5;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			})
			.AddEntityFrameworkStores<VotingDbContext>().AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "mvccookie";
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
				options.SlidingExpiration = true;
			});

			builder.Services.AddTransient<IPollService, PollService>();

			builder.Services.AddAutoMapper(typeof(Program));

			builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen();
		
			var app = builder.Build();

            if (app.Environment.IsDevelopment())
			{ 
				app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();

            app.MapControllers();

			using (var serviceScope = app.Services.CreateScope())
			{
				// Adatbázis inicializálása
				DbInitializer.Initialize(serviceScope.ServiceProvider);
			}

			app.Run();
        }
    }
}