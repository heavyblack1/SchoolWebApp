using Microsoft.EntityFrameworkCore;
using SchoolWebApp.Data;
using SchoolWebApp.Services;

namespace SchoolWebApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:SchoolDbConnection"]);
            });
            //builder.Services.AddDbContext<ApplicationDbContext>(options => {
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection"));
            //});
            builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<SubjectService>();
            builder.Services.AddScoped<GradesService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Handle 404 page
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404) {
                    context.Request.Path = "/NotFound";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Students}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
