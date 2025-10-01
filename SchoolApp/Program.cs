using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Repositories;
using Serilog;

namespace SchoolApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            connString = connString!.Replace("{DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS") ?? "");
                

            builder.Services.AddDbContext<Mvc8DbProContext>(options => options.UseSqlServer(connString));
            builder.Services.AddRepositories();

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<Configuration.MapperConfig>());
            builder.Host.UseSerilog((context, config) =>
            {
               config.ReadFrom.Configuration(context.Configuration);
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
