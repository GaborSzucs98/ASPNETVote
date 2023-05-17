using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MVCapp.Persitence.Services;
using MVCapp.Persitence;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MVCapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<VotingDbContext>(options =>
            {
                IConfigurationRoot configuration = builder.Configuration;

                options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));

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
            .AddEntityFrameworkStores<VotingDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "mvccookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
            });



            builder.Services.AddTransient<IPollService, PollService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");

            using (var serviceScope = app.Services.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<VotingDbContext>())
            {
                DbInitializer.Initialize(context);
            }

            app.Run();
        }
    }
}